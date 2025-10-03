using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IGamerLogic
    {
        Task<Gamer> GetGamerById(Guid gamerId);
        Task<Gamer> GetGamerByEmail(string gamerEmail);
        Task<Gamer> AddGamer(NewGamer gamer);
    }
}