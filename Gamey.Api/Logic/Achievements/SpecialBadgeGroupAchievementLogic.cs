using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class SpecialBadgeGroupAchievementLogic(IAchievementsLogic _achievementsLogic) : AwardsLogic, ISpecialBadgeGroupLogic
    {   
        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {
            if (gamer.id == null) throw new ArgumentNullException(nameof(gamer.id), "Gamer ID cannot be null");
            if(badgeGroupDetails.id == null) throw new ArgumentNullException(nameof(badgeGroupDetails.id), "Badge Group ID cannot be null");

            // special badge groups only have 1 badge!
            var badge = badgeGroupDetails.BadgeDetails.Single();

            // achievement already exists           
            if (await _achievementsLogic.Exists(gamer.id.Value, badge.id.Value)) return null; // don't error just say we haven't earned anything here
                
            return badge; // return details of the actual badge achieved
        }
    }
}
