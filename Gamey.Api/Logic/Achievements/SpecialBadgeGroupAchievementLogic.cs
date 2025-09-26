using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class SpecialBadgeGroupAchievementLogic(IAchievementsLogic _achievementsLogic) : AwardsLogic, ISpecialBadgeGroupLogic
    {   
        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {
            // special badge groups only have 1 badge!
            var badge = badgeGroupDetails.BadgeDetails.Single();

            // achievement already exists           
            if (await _achievementsLogic.Exists(gamer, badge.Id)) return null;
                
            return badge; // return details of the actual badge achieved
        }
    }
}
