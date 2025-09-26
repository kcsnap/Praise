using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GameDataLayer(ILogger<GameDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IGameDataLayer
    {
        public async Task<Game> Save(Game game)
        {
            try
            {
                _logger.LogInformation("Gamey Game.Save() " + game.id);

                // if new item assign id
                if (game.id == null || game.id == Guid.Empty)
                    game.id = Guid.NewGuid();

                ItemResponse<Game> response = await _gameyDbConnection.GameContainer.UpsertItemAsync<Game>(
                    item: game,
                    partitionKey: new PartitionKey(game.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "Gamey Error in GameDataLayer.Save (praise id = " + game.id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<Game> ReadByClientId(string clientId)
        {
            try
            {
                var query = "SELECT * FROM game g WHERE g.ClientId = @clientId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@clientId", clientId);

                // Query multiple items from container
                using FeedIterator<Game> filteredFeed = _gameyDbConnection.GameContainer.GetItemQueryIterator<Game>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("Game details not found for clientid:" + clientId);
                
                FeedResponse<Game> dbGames = await filteredFeed.ReadNextAsync();

                if (dbGames == null || dbGames.Count() == 0) throw new Exception("Unable to find Games for ClientId: " + clientId);
                if (dbGames.Count() > 1) throw new Exception("More than one game found for ClientId: " + clientId);

                return dbGames.Single();
            }
            catch(Exception ex)
            {
                var error = "Error in GameDataLayer.Read (clientid = " + clientId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        //protected async Task<IEnumerable<Game>> ReadByEmail(string email, bool isSender, int? pageNumber, int? numberPerPage)
        //{
        //    try
        //    {
        //        var response = new List<Game>();
        //        var query = isSender ?
        //            "SELECT * FROM PraiseContainer p WHERE p.FromEmail = @email ORDER BY p.PraiseDate DESC"
        //            : "SELECT * FROM PraiseContainer p WHERE p.ToEmail = @email ORDER BY p.PraiseDate DESC";

        //        // only add pagination if both values are provided
        //        if (pageNumber.HasValue && numberPerPage.HasValue)
        //        {
        //            int offset = (pageNumber.Value - 1) * numberPerPage.Value;
        //            query += $" OFFSET {offset} LIMIT {numberPerPage.Value}";
        //        }

        //        // Build query definition
        //        var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@email", email);

        //        // Query multiple items from container
        //        using FeedIterator<PraiseDetails> filteredFeed = praiseContainer.GetItemQueryIterator<PraiseDetails>(
        //            queryDefinition: parameterizedQuery
        //        );

        //        // Iterate query result pages
        //        while (filteredFeed.HasMoreResults)
        //        {
        //            FeedResponse<PraiseDetails> dbPraises = await filteredFeed.ReadNextAsync();

        //            // Iterate query results
        //            foreach (PraiseDetails item in dbPraises)
        //            {
        //                response.Add(item);
        //            }
        //        }

        //        return response;
        //    }
        //    catch(Exception ex)
        //    {
        //        var error = "Error in PraiseDetailsDataLayer.ReadByEmail (email = " + email + ")";
        //        _logger.LogError(ex, error);
        //        throw new Exception(error, ex);
        //    }
        //}
    }
}
