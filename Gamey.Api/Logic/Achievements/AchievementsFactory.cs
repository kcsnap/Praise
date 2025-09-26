using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class AchievementsFactory(ISpecialBadgeGroupLogic _specialBadgeGroupLogic) : IAchievementsFactory
    {
        public IAwardsLogic Get(BadgeGroupDetails badgeGroupDetails)
        {
            switch (badgeGroupDetails.BadgeType)
            {
                case BadgeType.Special:
                    return _specialBadgeGroupLogic;
                //case BadgeType.Count:
                //    return new CountBadgeGroupLogic();
                //case BadgeType.Streak:
                //    return new StreakBadgeGroupLogic();
                default:
                    throw new NotImplementedException("Badge type not implemented");

            }
        }
    }

}
