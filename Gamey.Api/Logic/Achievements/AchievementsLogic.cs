using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class AchievementsLogic : IAchievementsLogic
    {
        protected IList<Achievement> Achievements { get; private set; }

        public AchievementsLogic()
        {
            Achievements = new List<Achievement>()
            {
                new Achievement
                {
                    Id = "achievement-id-1",
                    BadgeId = "badge-id-1",
                    GamerId = "kenneth.cox@snapwire.co.uk",
                    DateAchieved = DateTime.UtcNow,
                    DateClaimed = null
                }
            };
        }

        public async Task<IEnumerable<Achievement>> GetAchievementsForGamer(string gamerId)
        {
            return Achievements.Where(a => a.GamerId == gamerId);
        }

        public async Task<Achievement> UpdateAchievement(string gamerId, string achievementId)
        {
            // TODO 

            return Achievements.FirstOrDefault(a => a.GamerId == gamerId && a.Id == achievementId);
        }

        public async Task<Achievement> SaveAchievement(Achievement achievement)
        {
            // TODO 

            return achievement;
        }

        public async Task<bool> Exists(Gamer gamer, string badgeId)
        {
            // special badge groups only have 1 badge!
            return string.IsNullOrEmpty(badgeId) != null ?
                Achievements.Any(a => a.GamerId == gamer.Id && a.BadgeId == badgeId) :
                false;
        }
    }
}
