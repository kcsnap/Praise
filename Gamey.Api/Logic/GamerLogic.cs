using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class GamerLogic : IGamerLogic
    {
        protected IList<Gamer> Gamers { get; private set; }

        public GamerLogic()
        {
            // load the available events (eventually move to database so can be configured per user registered!)
            Task.Run(LoadGamers);
        }

        protected async Task LoadGamers()
        {
            Gamers = new List<Gamer>
            {
                new Gamer
                {
                    Id = "unique-gamer-1", 
                    GamerName = "randomly-assigned-username-1",
                    GamerEmail = "kenneth.cox@snapwire.co.uk",
                    GameId = "praise-game"
                },
                new Gamer
                {
                    Id = "unique-gamer-2",
                    GamerName = "randomly-assigned-username-2",
                    GamerEmail = "gavin@snapwire.co.uk",
                    GameId = "praise-game"
                },
                new Gamer
                {
                    Id = "unique-gamer-3",
                    GamerName = "randomly-assigned-username-3",
                    GamerEmail = "sarah@snapwire.co.uk",
                    GameId = "praise-game"
                }
            };
        }

        public async Task<Gamer> GetGamerById(string gamerName)
        {
            return Gamers.FirstOrDefault(e => e.GamerName == gamerName);
        }

        public async Task<Gamer> GetGamerByEmail(string gamerEmail)
        {
            return Gamers.FirstOrDefault(e => e.GamerEmail == gamerEmail);
        }

        public async Task<Gamer> UpdateGamer(string gamerEmail, Gamer gamer)
        {
            throw new NotImplementedException();
        }

        public async Task<Gamer> AddGamer(Gamer gamer)
        {
            throw new NotImplementedException();
        }
    }
}
