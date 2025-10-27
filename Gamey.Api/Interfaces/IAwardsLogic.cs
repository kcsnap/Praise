using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public interface IAwardsLogic
    {
        Task<GamerEventData> ConfirmEvent(Guid gamerId, Guid gameId, BadgeGroupDetails badgeGroupDetails);
        Task<BaseBadgeDetails> AwardBadge(Gamer gamer, GamerEventData gamerEventData, BadgeGroupDetails badgeGroupDetails);
    }

    public interface ISpecialBadgeGroupLogic : IAwardsLogic;
    public interface ICountBadgeGroupLogic : IAwardsLogic;
    public interface IStreakBadgeGroupLogic : IAwardsLogic;
}