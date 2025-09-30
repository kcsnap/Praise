using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class AchievementsLogic(IAchievementDataLayer _achievementsDataLayer) : IAchievementsLogic
    {
        public async Task<IEnumerable<Achievement>> GetAchievementsForGamer(Guid gamerId)
        {
            return await _achievementsDataLayer.Read(gamerId);
        }

        public async Task<Achievement> UpdateAchievement(Guid gamerId, Guid achievementId)
        {
            // should be exactly 1
            var achievements = await _achievementsDataLayer.Read(gamerId);
            var achievement = achievements.SingleOrDefault(a => a.id == achievementId && a.GamerId == gamerId);

            // check we've found achievement
            if (achievement == null) throw new Exception("Can't find achievement for achievementid: " + achievementId + " and gamerid: " + gamerId);

            achievement.DateClaimed = DateTime.UtcNow;
            return await _achievementsDataLayer.Save(achievement);
        }

        public async Task<Achievement> AddAchievement(Guid gamerId, Guid badgeId)
        {
            return await _achievementsDataLayer.Save(new Achievement
            {
                GamerId = gamerId, 
                BadgeId = badgeId,
                DateAchieved = DateTime.UtcNow                
            });
        }

        public async Task<bool> Exists(Guid gamerId, Guid badgeId)
        {
            var achievements = await _achievementsDataLayer.Read(gamerId);
            return achievements.Any(a => a.BadgeId == badgeId);
        }
    }
}
