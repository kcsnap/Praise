using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Gamey.Api.DataLayer.Interfaces;

namespace Gamey.Api.Logic
{
    public class GameLogic : IGameLogic
    {
        protected readonly Game Game;
        protected readonly IGameDataLayer _gameDataLayer;
        
        public GameLogic(IHttpContextAccessor _contextAccessor, IGameDataLayer gameDataLayer)
        {
            _gameDataLayer = gameDataLayer;

            // we don't need user here, we just need clientid from authenticated client
            var clientId = _contextAccessor.HttpContext.Request.Headers["X-CLIENT-ID"];
            if(string.IsNullOrEmpty(clientId)) throw new Exception("Invalid ClientId" + clientId);

            // get game associated to the client id
            Game = Task.Run(async() => await _gameDataLayer.ReadByClientId(clientId)).Result;
        }

        public async Task<Game> GetGame()
        {            
            if (Game == null) throw new Exception("Game not configured");
            return Game;
        }
    }
}
