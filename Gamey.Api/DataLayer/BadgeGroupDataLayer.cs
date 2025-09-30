using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{
    public class BadgeGroupDataLayer(ILogger<BadgeGroupDataLayer> _logger, IGameyDbConnection _gameyDbConnection, IBadgeDataLayer badgeDataLayer) : IBadgeGroupDataLayer
    {
        public async Task<IEnumerable<BadgeGroupDetails>> Read(Guid gameId)
        {
            var response = new List<BadgeGroupDetails>();

            try
            {
                var query = "SELECT * FROM BadgeGroups g WHERE g.GameId = @gameId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gameId", gameId.ToString());

                // Query multiple items from container
                using FeedIterator<BadgeGroupDetails> filteredFeed = _gameyDbConnection.BadgeGroupContainer.GetItemQueryIterator<BadgeGroupDetails>(
                    queryDefinition: parameterizedQuery
                );

                while (filteredFeed.HasMoreResults)
                {
                    //FeedResponse<BadgeGroupDetails> dbBadgeGroups = await filteredFeed.ReadNextAsync();
                    // we need to capture derived types here....
                    FeedResponse<BadgeGroupDetails> dbBadgeGroups = await filteredFeed.ReadNextAsync();

                    // Iterate query results
                    foreach (BadgeGroupDetails item in dbBadgeGroups)
                    {
                        // iterate badges now and cast as required...
                        item.BadgeDetails = (await badgeDataLayer.Read(item.id.Value, item.BadgeType)).ToList();
                        response.Add(item);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                var error = "Error in AchievementDataLayer.Read (gameId = " + gameId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<IEnumerable<BadgeGroupDetails>> ReadByEventCode(Guid gameId, Guid gameEventCodeId)
        {
            var response = new List<BadgeGroupDetails>();

            try
            {
                var query = "SELECT * FROM BadgeGroups g WHERE g.GameId = @gameId AND g.GameEventCodeId = @gameEventCodeId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query)
                                                .WithParameter("@gameId", gameId.ToString())
                                                .WithParameter("@gameEventCodeId", gameEventCodeId.ToString());

                // Query multiple items from container
                using FeedIterator<BadgeGroupDetails> filteredFeed = _gameyDbConnection.BadgeGroupContainer.GetItemQueryIterator<BadgeGroupDetails>(
                    queryDefinition: parameterizedQuery
                );

                while (filteredFeed.HasMoreResults)
                {
                    FeedResponse<BadgeGroupDetails> dbBadgeGroups = await filteredFeed.ReadNextAsync();

                    // Iterate query results
                    foreach (BadgeGroupDetails item in dbBadgeGroups)
                    {
                        // iterate badges now and cast as required...
                        item.BadgeDetails = (await badgeDataLayer.Read(item.id.Value, item.BadgeType)).ToList();
                        response.Add(item);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                var error = "Error in AchievementDataLayer.Read (gameId = " + gameId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
