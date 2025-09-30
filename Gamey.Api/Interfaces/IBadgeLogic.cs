using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IBadgeLogic
    {
        Task<IEnumerable<BadgeGroupDetails>> GetBadgesAvailableForGame(Guid gameId);
        Task<IEnumerable<BaseBadgeDetails>> GetBadgesEarnedForEvent(Guid gameId, Guid gamerId, GameEvent gameEventCode);

    }
}