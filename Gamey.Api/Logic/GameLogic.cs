using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Gamey.Api.DataLayer.Interfaces;

namespace Gamey.Api.Logic
{
    public class GameLogic : IGameLogic
    {
        protected Game Game;
        protected readonly IGameDataLayer _gameDataLayer;
        
        public GameLogic(IHttpContextAccessor _contextAccessor, IGameDataLayer gameDataLayer)
        {
            _gameDataLayer = gameDataLayer;
        }

        public async Task<Game> GetGame()
        {
            if (Game == null) throw new Exception("Game not configured");
            return Game;
        }

        public async Task<Game> SetGame(string clientId, string apiKey)
        {
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey)) throw new ArgumentException("clientid and/or apikey cannot be null");

            Game = await _gameDataLayer.ReadByClientIdAndApiKey(clientId, apiKey);

            if (Game == null) throw new Exception("Game not configured");
            return Game;
        }
    }
}
