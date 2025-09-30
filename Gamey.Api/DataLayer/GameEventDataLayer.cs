using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GameEventDataLayer(ILogger<GameEventDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IGameEventDataLayer
    {

        public async Task<IEnumerable<GameEvent>> Read(Guid gameId)
        {
            var response = new List<GameEvent>();

            try
            {
                var query = "SELECT * FROM GameEvents g WHERE g.GameId = @gameId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gameId", gameId.ToString());

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

        public async Task<GameEvent> ReadByEventCode(Guid gameId, string gameEventCode)
        {
            var response = new List<GameEvent>();

            try
            {
                var query = "SELECT * FROM GameEvents g WHERE g.GameId = @gameId AND g.Code = @gameEventCode";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query)
                                                .WithParameter("@gameId", gameId.ToString())
                                                .WithParameter("@gameEventCode", gameEventCode);

                // Query multiple items from container
                using FeedIterator<GameEvent> filteredFeed = _gameyDbConnection.GameEventContainer.GetItemQueryIterator<GameEvent>(
                    queryDefinition: parameterizedQuery
                );

                if (!filteredFeed.HasMoreResults) throw new Exception("Game Event details not found for gameeventcode:" + gameEventCode + " and gameid: " + gameId);
                
                FeedResponse<GameEvent> dbGameEvents = await filteredFeed.ReadNextAsync();
                if (dbGameEvents == null || dbGameEvents.Count() == 0) throw new Exception("Unable to find Games for gameeventcode:" + gameEventCode + " and gameid: " + gameId);
                if (dbGameEvents.Count() > 1) throw new Exception("More than one game found for gameeventcode:" + gameEventCode + " and gameid: " + gameId);

                // should be only 1
                return dbGameEvents.Single();
            }
            catch (Exception ex)
            {
                var error = "Error in GameEventDataLayer.Read (gameId = " + gameId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
