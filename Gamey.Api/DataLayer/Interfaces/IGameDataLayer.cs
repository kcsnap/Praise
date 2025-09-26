using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGameDataLayer
    {
        Task<Game> ReadByClientId(string clientId);
        Task<Game> Save(Game game);
    }
}