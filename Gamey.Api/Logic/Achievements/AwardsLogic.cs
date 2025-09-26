using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public abstract class AwardsLogic : IAwardsLogic
    {
        public abstract Task<BaseBadgeDetails> AwardBadge(Gamer gamer, BadgeGroupDetails badgeGroupDetails);
    }

}
