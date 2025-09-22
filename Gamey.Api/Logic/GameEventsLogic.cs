using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class GameEventsLogic : IGameEventsLogic
    {
        protected IList<GameEventCode> GameEvents { get; private set; }

        public GameEventsLogic()
        {
            // load the available events (eventually move to database so can be configured per user registered!)
            Task.Run(LoadGameEvents);
        }

        protected async Task LoadGameEvents()
        {
            GameEvents = new List<GameEventCode>
            {
                new GameEventCode
                {
                    Id = "gamer-registered",
                    Code = "register-event",
                    Description = "captures the gamers registration event",
                    Name = "Gamer Registration Event"
                },
                new GameEventCode
                {
                    Id = "gamer-login",
                    Code = "login-event",
                    Description = "captures the gamers login event",
                    Name = "Gamer Login Event"
                },
                new GameEventCode
                {
                    Id = "gamer-send-praise",
                    Code = "send-praise-event",
                    Description = "captures the gamers send praise event",
                    Name = "Gamer Send Praise Event"
                }
            };
        }

        public async Task<GameEventCode> GetGameEventByCode(string eventCode)
        {
            return GameEvents.FirstOrDefault(e => e.Code == eventCode);
        }

    }
}
