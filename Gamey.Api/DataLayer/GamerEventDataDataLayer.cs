using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GamerEventDataDataLayer(ILogger<GameDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IGamerEventDataDataLayer
    {
        public async Task<GamerEventData> Read(Guid gamerId, Guid gameEventId)
        {
            var response = new List<GamerEventData>();

            try
            {
                var query = "SELECT * FROM GamerEventData g WHERE g.GamerId = @gamerId AND g.GameEventId = @gameEventId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query).WithParameter("@gamerId", gamerId.ToString())
                                                                   .WithParameter("@gameEventId", gameEventId.ToString());

                // Query multiple items from container as raw JSON
                using var filteredFeed = _gameyDbConnection.GamerEventDataContainer.GetItemQueryIterator<GamerEventData>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("GameEventdataDataLayer.Save() - GameEventdata details not found for gamerId:" + gamerId + " and Event: " + gameEventId);

                FeedResponse<GamerEventData> dbGamerEventDatas = await filteredFeed.ReadNextAsync();

                if (dbGamerEventDatas.Count() > 1) throw new Exception("GameEventdataDataLayer.Save() - More than one gameeventdata found for gamerId:" + gamerId + " and Event: " + gameEventId);
                if (dbGamerEventDatas == null || dbGamerEventDatas.Count() == 0) return null; // valid scenario checking if user exists

                return dbGamerEventDatas.Single();
            }
            catch (Exception ex)
            {
                var error = "GameEventdataDataLayer.Save() - unknown error for:" + gamerId + " and Event: " + gameEventId;
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        
        public async Task<GamerEventData> Save(GamerEventData gamerEventData)
        {
            try
            {
                _logger.LogInformation("gamerEventData.Save() " + gamerEventData.id);

                // if new item assign id
                if (gamerEventData == null) throw new Exception("gamerEventDataDataLayer() - gamer cannot be null");
                
                if (gamerEventData.id == null || gamerEventData.id == Guid.Empty) gamerEventData.id = Guid.NewGuid();

                ItemResponse<GamerEventData> response = await _gameyDbConnection.GamerEventDataContainer.UpsertItemAsync<GamerEventData>(
                    item: gamerEventData,
                    partitionKey: new PartitionKey(gamerEventData.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "Gamey Error in GamerEventDataDataLayer.Save (GamerEventData id = " + gamerEventData.id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
