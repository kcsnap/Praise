using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using System.Diagnostics.Eventing.Reader;

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

            // does achievement already exist
            if (await _achievementsLogic.Exists(gamer.id.Value, badge.id.Value)) return null; // don't error just say we haven't earned anything here
                
            return badge; // return details of the actual badge achieved
        }
    }

    public class CountBadgeGroupLogic(IAchievementsLogic _achievementsLogic, IGamerEventDataDataLayer _gamerEventDataDataLayer) : AwardsLogic, ICountBadgeGroupLogic
    {
        /// <summary>
        /// badges are awarded based on a count of events - for Praise this is total number of praises sent
        /// </summary>
        /// <param name="gamer"></param>
        /// <param name="badgeGroupDetails"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {
            if(gamer == null || badgeGroupDetails == null) throw new ArgumentNullException("Gamer and BadgeGroupDetails cannot be null");
            if (badgeGroupDetails.id == null) throw new ArgumentNullException(nameof(badgeGroupDetails.id), "Badge Group ID cannot be null");

            // reteieve the current count for this event and gamer
            var gamerEventData = await _gamerEventDataDataLayer.Read(gamer.id.Value, badgeGroupDetails.GameEventCodeId);

            if(gamerEventData == null)
            {
                // no record exists so create one
                gamerEventData = new GamerEventData
                {
                    GamerId = gamer.id.Value,
                    GameEventId = badgeGroupDetails.GameEventCodeId,
                    Count = 0
                };
            }

            // +1 to the count for this event, 
            gamerEventData.Count++;
            gamerEventData.LastEventDate = DateTime.UtcNow;

            // save the updated count
            gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);

            // check if any badge thresholds are met
            var badgeToAward = badgeGroupDetails.BadgeDetails
                                        .Where(b => (b as BadgeDetailsCount).CountThreshold == gamerEventData.Count) // find any badge with a threshold matching the current count
                                        .SingleOrDefault(); // should only be one or none

            // if no badges return null
            if (badgeToAward == null) return null;

            // does achievement already exist
            if (await _achievementsLogic.Exists(gamer.id.Value, badgeToAward.id.Value)) return null; // don't error just say we haven't earned anything here

            // if threshold met - return new badge
            return badgeToAward; // could be null if no matches
        }
    }

    public class StreakBadgeGroupLogic(IAchievementsLogic _achievementsLogic, IGamerEventDataDataLayer _gamerEventDataDataLayer) : AwardsLogic, IStreakBadgeGroupLogic
    {
        // Streak badges are awarded based on a count of consecutive events - for Praise this is number of consecutive days with praises sent
        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {
            if (gamer == null || badgeGroupDetails == null) throw new ArgumentNullException("Gamer and BadgeGroupDetails cannot be null");
            if (badgeGroupDetails.id == null) throw new ArgumentNullException(nameof(badgeGroupDetails.id), "Badge Group ID cannot be null");

            // reteieve the current count for this event and gamer
            var gamerEventData = await _gamerEventDataDataLayer.Read(gamer.id.Value, badgeGroupDetails.GameEventCodeId);

            if (gamerEventData == null)
            {
                // no record exists so create one
                gamerEventData = new GamerEventData
                {
                    GamerId = gamer.id.Value,
                    GameEventId = badgeGroupDetails.GameEventCodeId,
                    Count = 0
                };
            }

            // +1 to the count for this event, ONLY if the event is on a consecutive day
            if (!gamerEventData.LastEventDate.HasValue ||
                    gamerEventData.LastEventDate.Value.Date != DateTime.Now.Date)
            { 
                // reset as this is not a consecutive day
                gamerEventData.Count = 1;
            }
            // if yesterday was last event increment streak count
            else if (gamerEventData.LastEventDate.Value.Date == DateTime.Now.Date.AddDays(-1))
            {
                gamerEventData.Count++;
            }

            // update last event date (regardless of whether count increased or reset)
            gamerEventData.LastEventDate = DateTime.UtcNow;

            // save the updated count
            gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);

            // check if any badge thresholds are met
            var badgeToAward = badgeGroupDetails.BadgeDetails
                                        .Where(b => (b as BadgeDetailsStreak).CountThreshold == gamerEventData.Count) // find any badge with a threshold matching the current count
                                        .SingleOrDefault(); // should only be one

            // if no badges return null
            if (badgeToAward == null) return null;

            // does achievement already exist
            if (await _achievementsLogic.Exists(gamer.id.Value, badgeToAward.id.Value)) return null; // don't error just say we haven't earned anything here

            // if threshold met - return new badge
            return badgeToAward; // could be null if no matches
        }
    }
}
