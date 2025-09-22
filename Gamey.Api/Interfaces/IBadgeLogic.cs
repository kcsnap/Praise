using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IBadgeLogic
    {
        Task<IList<BadgeGroupDetails>> CheckBadges(string gamerId, GameEventCode gameEventCode);
    }
}