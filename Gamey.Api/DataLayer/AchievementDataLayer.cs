using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{
    public class AchievementDataLayer(ILogger<AchievementDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IAchievementDataLayer
    {
        public async Task<IEnumerable<Achievement>> Read(Guid gamerId)
        {
            var response = new List<Achievement>();

            try
            {
                var query = "SELECT * FROM Achievements g WHERE g.GamerId = @gamerId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gamerId", gamerId.ToString());

                // Query multiple items from container
                using FeedIterator<Achievement> filteredFeed = _gameyDbConnection.AchievementContainer.GetItemQueryIterator<Achievement>(
                    queryDefinition: parameterizedQuery
                );

                while (filteredFeed.HasMoreResults)
                {
                    FeedResponse<Achievement> dbAchievements = await filteredFeed.ReadNextAsync();

                    // Iterate query results
                    foreach (Achievement item in dbAchievements)
                    {
                        response.Add(item);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                var error = "Error in AchievementDataLayer.Read (gamerId = " + gamerId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<Achievement> Save(Achievement achievement)
        {
            try
            {
                // if new item assign id
                if (achievement.id == null || achievement.id == Guid.Empty)
                    achievement.id = Guid.NewGuid();

                _logger.LogInformation("Gamey Achievement.Save() " + achievement.id);

                ItemResponse<Achievement> response = await _gameyDbConnection.AchievementContainer.UpsertItemAsync<Achievement>(
                    item: achievement,
                    partitionKey: new PartitionKey(achievement.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "Gamey Error in AchievementDataLayer.Save (achievment id = " + achievement.id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
