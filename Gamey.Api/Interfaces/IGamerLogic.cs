using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IGamerLogic
    {
        Task<Gamer> GetGamerByUniqueName(string gamerName);
    }
}