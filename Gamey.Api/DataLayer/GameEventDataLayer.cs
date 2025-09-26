using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GameEventDataLayer(ILogger<GameEventDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IGameEventDataLayer
    {
        //public async Task<Game> Save(Game game)
        //{
        //    try
        //    {
        //        _logger.LogInformation("PRAISE PraiseDetailsDataLayer.Save() " + game.id);

        //        // if new item assign id
        //        if (game.id == null || game.id == Guid.Empty)
        //            game.id = Guid.NewGuid();

        //        ItemResponse<Game> response = await _gameyDbConnection.GameContainer.UpsertItemAsync<Game>(
        //            item: game,
        //            partitionKey: new PartitionKey(game.id.ToString())
        //        );

        //        return response;
        //    }
        //    catch (CosmosException ex)
        //    {
        //        var error = "PRAISE Error in GameDataLayer.Save (praise id = " + game.id + ")";
        //        _logger.LogError(ex, error);
        //        throw new Exception(error, ex);
        //    }
        //}

        public async Task<IEnumerable<GameEvent>> Read(Guid gameId)
        {
            var response = new List<GameEvent>();

            try
            {
                var query = "SELECT * FROM GameEvents g WHERE g.GameId = @gameId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gameId", gameId.ToString().ToUpper());

                // Query multiple items from container
                using FeedIterator<GameEvent> filteredFeed = _gameyDbConnection.GameEventContainer.GetItemQueryIterator<GameEvent>(
                    queryDefinition: parameterizedQuery
                );

                while (filteredFeed.HasMoreResults)
                {
                    FeedResponse<GameEvent> dbGameEvents = await filteredFeed.ReadNextAsync();

                    // Iterate query results
                    foreach (GameEvent item in dbGameEvents)
                    {
                        response.Add(item);
                    }
                }

                return response;
            }
            catch(Exception ex)
            {
                var error = "Error in GameEventDataLayer.Read (gameId = " + gameId + ")";
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
