using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class GamerLogic(IGamerDataLayer _gamerDataLayer) : IGamerLogic
    {
        public async Task<Gamer> GetGamerById(Guid gamerId)
        {
            return await _gamerDataLayer.ReadByGamerId(gamerId);
        }

        public async Task<Gamer> GetGamerByEmail(string gamerEmail)
        {
            return await _gamerDataLayer.ReadByGamerEmail(gamerEmail);
        }

        public async Task<Gamer> AddGamer(Gamer gamer)
        {
            throw new NotImplementedException();
        }
    }
}
