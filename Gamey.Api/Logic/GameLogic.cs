using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Gamey.Api.DataLayer.Interfaces;

namespace Gamey.Api.Logic
{
    public class GameLogic(IGameDataLayer _gameDataLayer) : IGameLogic
    {
        protected Game Game { get; private set; }

        //public GameLogic()
        //{
        //    // load the available events (eventually move to database so can be configured per user registered!)
        //    //LoadGame();
        //}

        //protected async Task LoadGame()
        //{
        //    //Game = new Game
        //    //{ 
        //    //    id = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210"),
        //    //    ClientId = "_clientid",
        //    //    EncryptedApiKey = "123",
        //    //    GameName = "Praise",
        //    //    GameDescription = "Gamification of Praise"
        //    //};
        //}

        public async Task<Game> GetGame(string clientId)
        {
            // get game associated to the client id
            Game = await _gameDataLayer.ReadByClientId(clientId);

            if (Game == null) throw new Exception("Game not configured");

            if(Game.ClientId == clientId)
                return Game;
            else
                throw new Exception("Invalid ClientId" + clientId);
        }
    }
}
