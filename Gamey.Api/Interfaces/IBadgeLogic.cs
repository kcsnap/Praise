using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IBadgeLogic
    {
        Task<IEnumerable<BadgeGroupDetails>> GetBadgesAvailableForGame(Guid gameId);
        Task<IEnumerable<BaseBadgeDetails>> GetBadgesEarnedForEvent(string gamerId, GameEvent gameEventCode);
    }
}