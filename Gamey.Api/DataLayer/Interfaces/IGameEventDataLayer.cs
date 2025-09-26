using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGameEventDataLayer
    {
        Task<IEnumerable<GameEvent>> Read(Guid gameId);

    }
}