using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IGameEventsLogic
    {
        Task<GameEventCode> GetGameEventByCode(string eventCode);
    }
}