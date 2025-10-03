using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class AchievementsFactory(ISpecialBadgeGroupLogic _specialBadgeGroupLogic, ICountBadgeGroupLogic _countBadgeGroupLogic, IStreakBadgeGroupLogic _streakBadgeGroupLogic) : IAchievementsFactory
    {
        public IAwardsLogic Get(BadgeGroupDetails badgeGroupDetails)
        {
            switch (badgeGroupDetails.BadgeType)
            {
                case BadgeType.Special:
                    return _specialBadgeGroupLogic;
                case BadgeType.Count:
                    return _countBadgeGroupLogic;
                case BadgeType.Streak:
                    return _streakBadgeGroupLogic;
                default:
                    throw new NotImplementedException("Badge type not implemented");
            }
        }
    }
}
