using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IGamerLogic
    {
        Task<Gamer> GetGamerById(string gamerName);
        Task<Gamer> GetGamerByEmail(string gamerEmail);
        Task<Gamer> UpdateGamer(string gamerEmail, Gamer gamer);
        Task<Gamer> AddGamer(Gamer gamer);
    }
}