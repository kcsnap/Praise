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
                    GamerName = "display-name-1"
                },
                new Gamer
                {
                    Id = "unique-gamer-2",
                    GamerName = "display-name-2"
                },
                new Gamer
                {
                    Id = "unique-gamer-3",
                    GamerName = "display-name-3"
                }
            };
        }

        public async Task<Gamer> GetGamerByUniqueName(string gamerName)
        {
            return Gamers.FirstOrDefault(e => e.GamerName == gamerName);
        }

    }
}
