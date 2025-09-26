using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Praise.Api.Configuration;
using Praise.Api.DataLayer.Interfaces;
using Praise.Models;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Praise.Api.DataLayer
{    
    public class PraiseDetailsDataLayer : IPraiseDetailsDataLayer
    {
        protected Database database;
        protected Container praiseContainer;
        protected CosmosClient client;
        protected ILogger<PraiseDetailsDataLayer> _logger;

        public PraiseDetailsDataLayer(IOptions<DatabaseOptions> dbOptions, ILogger<PraiseDetailsDataLayer> logger)
        {
            var primaryKey = dbOptions.Value.PrimaryKey;
            var connStr = dbOptions.Value.ConnectionString;
            _logger = logger;

            DefaultAzureCredential credential = new();

            client = new(
                connectionString: connStr
            );

            database = client.GetDatabase("Praise");
            praiseContainer = database.GetContainer("PraiseContainer");
        }

        public async Task<PraiseDetails> Save(PraiseDetails praiseDO)
        {
            try
            {
                _logger.LogInformation("PRAISE PraiseDetailsDataLayer.Save() " + praiseDO.id);

                // if new item assign id
                if (praiseDO.id == null || praiseDO.id == Guid.Empty)
                    praiseDO.id = Guid.NewGuid();

                ItemResponse<PraiseDetails> response = await praiseContainer.UpsertItemAsync<PraiseDetails>(
                    item: praiseDO,
                    partitionKey: new PartitionKey(praiseDO.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "PRAISE Error in PraiseDetailsDataLayer.Save (praise id = " + praiseDO.id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<PraiseDetails> Read(Guid id)
        {
            try
            {
                ItemResponse<PraiseDetails> response = await praiseContainer.ReadItemAsync<PraiseDetails>(
                    id: id.ToString(),
                    partitionKey: new PartitionKey(id.ToString())
                );

                return response;
            }
            catch(Exception ex)
            {
                var error = "Error in PraiseDetailsDataLayer.Read (id = " + id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<IEnumerable<PraiseDetails>> ReadByPraiseSenderEmail(string email, int? pageNumber, int? numberPerPage)
        {
            return await ReadByEmail(email, isSender: true, pageNumber, numberPerPage);
        }
        public async Task<IEnumerable<PraiseDetails>> ReadByPraiseReceiverEmail(string email, int? pageNumber, int? numberPerPage)
        {
            return await ReadByEmail(email, isSender: false, pageNumber, numberPerPage);
        }
        protected async Task<IEnumerable<PraiseDetails>> ReadByEmail(string email, bool isSender, int? pageNumber, int? numberPerPage)
        {
            try
            {
                var response = new List<PraiseDetails>();
                var query = isSender ?
                    "SELECT * FROM PraiseContainer p WHERE p.FromEmail = @email ORDER BY p.PraiseDate DESC"
                    : "SELECT * FROM PraiseContainer p WHERE p.ToEmail = @email ORDER BY p.PraiseDate DESC";

                // only add pagination if both values are provided
                if (pageNumber.HasValue && numberPerPage.HasValue)
                {
                    int offset = (pageNumber.Value - 1) * numberPerPage.Value;
                    query += $" OFFSET {offset} LIMIT {numberPerPage.Value}";
                }

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@email", email);

                // Query multiple items from container
                using FeedIterator<PraiseDetails> filteredFeed = praiseContainer.GetItemQueryIterator<PraiseDetails>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                while (filteredFeed.HasMoreResults)
                {
                    FeedResponse<PraiseDetails> dbPraises = await filteredFeed.ReadNextAsync();

                    // Iterate query results
                    foreach (PraiseDetails item in dbPraises)
                    {
                        response.Add(item);
                    }
                }

                return response;
            }
            catch(Exception ex)
            {
                var error = "Error in PraiseDetailsDataLayer.ReadByEmail (email = " + email + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
