using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{
    public class ActionHistoryDataLayer(ILogger<ActionHistoryDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IActionHistoryDataLayer
    {   
        public async Task<ActionHistory> Save(ActionHistory actionHistory)
        {
            try
            {
                // if new item assign id
                if(actionHistory == null) throw new Exception("ActionHistoryDataLayer() - action History cannot be null");
                if(!actionHistory.id.HasValue) actionHistory.id = Guid.NewGuid();

                _logger.LogInformation("Gamey ActionHistory.Save() " + actionHistory.id);

                ItemResponse<ActionHistory> response = await _gameyDbConnection.ActionHistoryContainer.UpsertItemAsync<ActionHistory>(
                    item: actionHistory,
                    partitionKey: new PartitionKey(actionHistory.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "Gamey Error in ActionHistoryDataLayer.Save (actionHistory id = " + actionHistory.id + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
