using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IActionHistoryLogic
    {
        Task AddAction(Game game, Gamer gamer, ActionEnum action);
    }
}