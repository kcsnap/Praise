using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public interface IAwardsLogic
    {
        Task<BaseBadgeDetails> AwardBadge(Gamer gamer, BadgeGroupDetails badgeGroupDetails);
    }

    public interface ISpecialBadgeGroupLogic : IAwardsLogic;
    public interface ICountBadgeGroupLogic : IAwardsLogic;
    public interface IStreakBadgeGroupLogic : IAwardsLogic;
}