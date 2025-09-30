using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGamerDataLayer
    {
        Task<Gamer> ReadByGamerId(Guid gamerId);
        Task<Gamer> ReadByGamerEmail(string gamerEmail);
    }
}