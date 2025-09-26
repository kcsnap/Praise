using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class GameEventsLogic(IGameEventDataLayer _gameEventDataLayer) : IGameEventsLogic
    {
        protected IList<GameEvent> GameEvents { get; private set; }

        //public GameEventsLogic()
        //{
        //    // load the available events (eventually move to database so can be configured per user registered!)
        //    //Task.Run(LoadGameEvents);
        //}

        //protected async Task LoadGameEvents()
        //{
        //    GameEvents = new List<GameEventCode>
        //    {
        //        new GameEventCode
        //        {
        //            id = "gamer-registered",
        //            Code = "register-event",
        //            Description = "captures the gamers registration event",
        //            Name = "Gamer Registration Event",
        //            GameId = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210")
        //        },
        //        new GameEventCode
        //        {
        //            id = "gamer-login",
        //            Code = "login-event",
        //            Description = "captures the gamers login event",
        //            Name = "Gamer Login Event",
        //            GameId = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210")
        //        },
        //        new GameEventCode
        //        {
        //            id = "gamer-send-praise",
        //            Code = "send-praise-event",
        //            Description = "captures the gamers send praise event",
        //            Name = "Gamer Send Praise Event",
        //            GameId = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210")
        //        }
        //    };
        //}

        public async Task<GameEvent> GetGameEventByCode(string eventCode)
        {
            return GameEvents.FirstOrDefault(e => e.Code == eventCode);
        }

        public async Task<IEnumerable<GameEvent>> GetGameEvents(Guid gameId)
        {
            return await _gameEventDataLayer.Read(gameId);
        }
    }
}
