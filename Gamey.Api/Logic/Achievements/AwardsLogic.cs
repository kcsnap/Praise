using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public abstract class AwardsLogic : IAwardsLogic
    {
        public abstract Task<GamerEventData> ConfirmEvent(Guid gamerId, Guid gameId, BadgeGroupDetails badgeGroupDetails);
        public abstract Task<BaseBadgeDetails> AwardBadge(Gamer gamer, GamerEventData gamerEventData, BadgeGroupDetails badgeGroupDetails);
    }

}
