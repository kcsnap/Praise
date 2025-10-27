using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Logic;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GameDataLayer(ILogger<GameDataLayer> _logger, IGameyDbConnection _gameyDbConnection, IDeterministicEncryption _deterministicEncryption) : IGameDataLayer
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

        public async Task<Game> ReadByClientIdAndApiKey(string clientId, string apiKey)
        {
            try
            {
                // encrypt the api key to compare with db value...
                var encryptedApiKey = await _deterministicEncryption.Encrypt(apiKey);

                var query = "SELECT * FROM game g WHERE g.ClientId = @clientId AND g.EncryptedApiKey = @encryptedApiKey";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@clientId", clientId)
                                                                          .WithParameter("@encryptedApiKey", encryptedApiKey);

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
    }
}
