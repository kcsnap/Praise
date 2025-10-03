using Gamey.Api.Models;
using Gamey.Models;

namespace Gamey.Api.Interfaces
{
    public interface IPrizeGroupLogic
    {
        Task<IEnumerable<PrizeGroups>> GetPrizesAvailableForGame(Guid gameId);
        Task<PrizeGroups> GetPrizeEarnedForEvent(Guid gameId, Guid gamerId, GameEvent gameEventCode);

    }
}