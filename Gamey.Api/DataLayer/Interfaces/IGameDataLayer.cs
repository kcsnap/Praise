using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGameDataLayer
    {
        Task<Game> ReadByClientIdAndApiKey(string clientId, string apiKey);
        Task<Game> Save(Game game);
    }
}