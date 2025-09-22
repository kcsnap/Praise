using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class SpecialAchievementsLogic
    {
        protected IList<Achievement> Achievements { get; private set; }

        public SpecialAchievementsLogic()
        {
            Achievements = new List<Achievement>()
            {
                new Achievement
                {
                    Id = "achievement-id-1",
                    BadgeId = "badge-id-1",
                    GamerId = "123",
                    DateAchieved = DateTime.UtcNow
                }
            };
        }

        public async Task<Achievement> IsAchievementAchieved(Gamer gamer, BaseBadgeDetails badgeDetails)
        {
            if (badgeDetails == null)
                throw new Exception("Badge Details cannot be null");

            var specialBadge = badgeDetails as BadgeDetailsSpecial;

            if (specialBadge != null)
                throw new Exception("BadgeDetails is not of type BadgeDetailsSpecial");

            // does achievement already exist?
            if(Achievements.Any(a => a.GamerId == gamer.Id && a.BadgeId == badgeDetails.Id))
                throw new Exception("Badge already earned");

            // any more checks needed? i.e. do we need to validate against Game (ie.e. companY) to ensure not achieving badges assigned to different game?

            // Save achievement to DB!!
            // TODO

            return new Achievement
            {
                Id = Guid.NewGuid().ToString(),
                BadgeId = badgeDetails.Id,
                GamerId = gamer.Id,
                DateAchieved = DateTime.UtcNow
            };
        }
    }
}
