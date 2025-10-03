using Gamey.Api.DataLayer.Interfaces;
using Gamey.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class PrizeGroupDataLayer(ILogger<PrizeGroupDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IPrizeGroupDataLayer
    {
        public async Task<IEnumerable<PrizeGroups>> Read(Guid gameId)
        {
            try
            {
                var query = "SELECT * FROM PrizeGroups pg WHERE pg.GameId = @gameId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gameId", gameId);

                // Query multiple items from container
                using FeedIterator<PrizeGroups> filteredFeed = _gameyDbConnection.GameContainer.GetItemQueryIterator<PrizeGroups>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("Game details not found for gameid:" + gameId);

                FeedResponse<PrizeGroups> dbPrizes = await filteredFeed.ReadNextAsync();

                if (dbPrizes == null || dbPrizes.Count() == 0) throw new Exception("Unable to find Games for gameid: " + gameId);
                if (dbPrizes.Count() > 1) throw new Exception("More than one game found for gameid: " + gameId);

                return dbPrizes;
            }
            catch (Exception ex)
            {
                var error = "Error in GamePrizeDataLayer.Read (gameEventCodeId = " + gameId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<PrizeGroups> Read(Guid gameId, Guid gameEventCodeId)
        {
            try
            {
                var query = "SELECT * FROM PrizeGroups pg WHERE pg.GameId = @gameId AND pg.GameEventCodeId = @gameEventCodeId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gameId", gameId)
                                                                          .WithParameter("@gameEventCodeId", gameEventCodeId);

                // Query multiple items from container
                using FeedIterator<PrizeGroups> filteredFeed = _gameyDbConnection.PrizeGroupsContainer.GetItemQueryIterator<PrizeGroups>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("Game details not found for gameEventCodeId:" + gameEventCodeId);
                
                FeedResponse<PrizeGroups> dbPrizes = await filteredFeed.ReadNextAsync();

                if (dbPrizes == null || dbPrizes.Count() == 0) return null; // no prizes available for this event
                if (dbPrizes.Count() > 1) throw new Exception("More than one prize found for gameEventCodeId: " + gameEventCodeId);

                return dbPrizes.Single();
            }
            catch(Exception ex)
            {
                var error = "Error in GamePrizeDataLayer.Read (gameEventCodeId = " + gameEventCodeId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
