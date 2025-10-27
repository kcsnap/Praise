using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IGameLogic
    {
        Task<Game> GetGame();
        Task<Game> SetGame(string clientId, string apiKey);
    }
}