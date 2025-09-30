using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class GamerDataLayer(ILogger<GameDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : IGamerDataLayer
    {
        //public async Task<Game> Save(Game game)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Gamey Game.Save() " + game.id);

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
        //        var error = "Gamey Error in GameDataLayer.Save (praise id = " + game.id + ")";
        //        _logger.LogError(ex, error);
        //        throw new Exception(error, ex);
        //    }
        //}

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

                if (dbGamers == null || dbGamers.Count() == 0) throw new Exception("Unable to find Games for gamerEmail: " + gamerEmail);
                if (dbGamers.Count() > 1) throw new Exception("More than one game found for gamerEmail: " + gamerEmail);

                return dbGamers.Single();
            }
            catch (Exception ex)
            {
                var error = "Error in GameDataLayer.Read (gamerEmail = " + gamerEmail + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
