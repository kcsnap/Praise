using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IGameEventsLogic
    {
        Task<GameEvent> GetGameEventByCode(string eventCode);
        Task<IEnumerable<GameEvent>> GetGameEvents(Guid gameId);
    }
}