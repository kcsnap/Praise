using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GamerDataLayer(ILogger<GameDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IGamerDataLayer
    {
        public async Task<Gamer> ReadByGamerId(Guid gamerId)
        {
            try
            {
                var query = "SELECT * FROM gamer g WHERE g.id = @gamerId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gamerId", gamerId);

                // Query multiple items from container
                using FeedIterator<Gamer> filteredFeed = _gameyDbConnection.GamerContainer.GetItemQueryIterator<Gamer>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("Game details not found for gamerid:" + gamerId);
                
                FeedResponse<Gamer> dbGamers = await filteredFeed.ReadNextAsync();

                if (dbGamers == null || dbGamers.Count() == 0) throw new Exception("Unable to find Games for ClientId: " + gamerId);
                if (dbGamers.Count() > 1) throw new Exception("More than one game found for gamerid: " + gamerId);

                return dbGamers.Single();
            }
            catch(Exception ex)
            {
                var error = "Error in GameDataLayer.Read (gamerid = " + gamerId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<Gamer> ReadByGamerEmail(string gamerEmail)
        {
            try
            {
                var query = "SELECT * FROM gamer g WHERE g.GamerEmail = @gamerEmail";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gamerEmail", gamerEmail);

                // Query multiple items from container
                using FeedIterator<Gamer> filteredFeed = _gameyDbConnection.GamerContainer.GetItemQueryIterator<Gamer>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("Game details not found for gamerEmail:" + gamerEmail);

                FeedResponse<Gamer> dbGamers = await filteredFeed.ReadNextAsync();

                if (dbGamers.Count() > 1) throw new Exception("More than one game found for gamerEmail: " + gamerEmail);
                if (dbGamers == null || dbGamers.Count() == 0) return null; // valid scenario checking if user exists

                return dbGamers.Single();
            }
            catch (Exception ex)
            {
                var error = "Error in GameDataLayer.Read (gamerEmail = " + gamerEmail + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<Gamer> Save(Gamer gamer)
        {
            try
            {
                _logger.LogInformation("Gamey Achievement.Save() " + gamer.id);

                // if new item assign id
                if (gamer == null) throw new Exception("GamerDataLayer() - gamer cannot be null");
                if(string.IsNullOrEmpty(gamer.GamerName)) throw new Exception("GamerDataLayer() - gamer name is required");
                if (string.IsNullOrEmpty(gamer.GamerEmail)) throw new Exception("GamerDataLayer() - gamer email is required");

                if (gamer.id == null || gamer.id == Guid.Empty) gamer.id = Guid.NewGuid();

                ItemResponse<Gamer> response = await _gameyDbConnection.GamerContainer.UpsertItemAsync<Gamer>(
                    item: gamer,
                    partitionKey: new PartitionKey(gamer.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "Gamey Error in AchievementDataLayer.Save (achievment id = " + gamer.id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
