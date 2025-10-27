using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Achievements
{
    public class SpecialBadgeGroupAchievementLogic(IAchievementsLogic _achievementsLogic, IGamerEventDataDataLayer _gamerEventDataDataLayer) : AwardsLogic, ISpecialBadgeGroupLogic
    {
        public async override Task<GamerEventData> ConfirmEvent(Guid gamerId, Guid gameId, BadgeGroupDetails badgeGroupDetails)
        {
            // reteieve the current count for this event and gamer
            var gamerEventData = await _gamerEventDataDataLayer.Read(gamerId, badgeGroupDetails.GameEventCodeId, badgeGroupDetails.id.Value);

            if (gamerEventData == null)
            {
                // no record exists so create one
                gamerEventData = new GamerEventData
                {
                    GamerId = gamerId,
                    GameEventId = badgeGroupDetails.GameEventCodeId,
                    BadgeGroupDetailsId = badgeGroupDetails.id.Value,
                    LastEventDate = DateTime.UtcNow
                };

                // save the updated count
                gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);
                return gamerEventData;
            }

            // no new event created as can be only 1
            return null;

        }

        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, GamerEventData gamerEventData, BadgeGroupDetails badgeGroupDetails)
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
        public async override Task<GamerEventData> ConfirmEvent(Guid gamerId, Guid gameId, BadgeGroupDetails badgeGroupDetails)
        {
            // reteieve the current count for this event and gamer
            var gamerEventData = await _gamerEventDataDataLayer.Read(gamerId, badgeGroupDetails.GameEventCodeId, badgeGroupDetails.id.Value);

            if (gamerEventData == null)
            {
                // no record exists so create one
                gamerEventData = new GamerEventData
                {
                    GamerId = gamerId,
                    GameEventId = badgeGroupDetails.GameEventCodeId,
                    BadgeGroupDetailsId = badgeGroupDetails.id.Value,
                    Count = 0
                };
            }

            // +1 to the count for this event, 
            gamerEventData.Count++;
            gamerEventData.LastEventDate = DateTime.UtcNow;

            // save the updated count
            gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);
            return gamerEventData;
        }

        /// <summary>
        /// badges are awarded based on a count of events - for Praise this is total number of praises sent
        /// </summary>
        /// <param name="gamer"></param>
        /// <param name="badgeGroupDetails"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, GamerEventData gamerEventData, BadgeGroupDetails badgeGroupDetails)
        {
            if(gamer == null || badgeGroupDetails == null) throw new ArgumentNullException("Gamer and BadgeGroupDetails cannot be null");
            if (badgeGroupDetails.id == null) throw new ArgumentNullException(nameof(badgeGroupDetails.id), "Badge Group ID cannot be null");

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
        public async override Task<GamerEventData> ConfirmEvent(Guid gamerId, Guid gameId, BadgeGroupDetails badgeGroupDetails)
        {
            // reteieve the current count for this event and gamer
            var gamerEventData = await _gamerEventDataDataLayer.Read(gamerId, badgeGroupDetails.GameEventCodeId, badgeGroupDetails.id.Value);

            if (gamerEventData == null)
            {
                // no record exists so create one
                gamerEventData = new GamerEventData
                {
                    GamerId = gamerId,
                    GameEventId = badgeGroupDetails.GameEventCodeId,
                    BadgeGroupDetailsId = badgeGroupDetails.id.Value,
                    LastEventDate = DateTime.Now,
                    MaxStreak = 1, // default both to 
                    CurrentStreak = 1 // 1 as first event
                };

                gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);
                return gamerEventData;
            }
            else // event is existing for this badge so lets check if streak continues
            {
                // if last event was today then streak already accounted for!
                if (gamerEventData.LastEventDate.Date == DateTime.Now.Date)
                {
                    return null;
                }
                // if not yesterday then we're not streaking so reset streak to 1, but do not return as not awardable
                else if (gamerEventData.LastEventDate.Date != DateTime.Now.Date.AddDays(-1))
                {
                    // last event was yesterday so increment current streak
                    gamerEventData.LastEventDate = DateTime.UtcNow;
                    gamerEventData.CurrentStreak = 1;

                    gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);
                    return null; // resetting, not awardable so don't return
                }
                else // is yesterday therefore update streak, and possibly max streak
                {
                    // last event was yesterday so increment current streak
                    gamerEventData.LastEventDate = DateTime.UtcNow;
                    gamerEventData.CurrentStreak++;

                    if (gamerEventData.CurrentStreak > gamerEventData.MaxStreak)
                    {
                        gamerEventData.MaxStreak = gamerEventData.CurrentStreak;
                    }

                    gamerEventData = await _gamerEventDataDataLayer.Save(gamerEventData);
                    return gamerEventData;
                }                
            }
        }

        // Streak badges are awarded based on a count of consecutive events - for Praise this is number of consecutive days with praises sent
        public async override Task<BaseBadgeDetails> AwardBadge(Gamer gamer, GamerEventData gamerEventData, BadgeGroupDetails badgeGroupDetails)
        {
            if (gamer == null || gamerEventData == null || badgeGroupDetails == null) throw new ArgumentNullException("Gamer and BadgeGroupDetails cannot be null");
            if (badgeGroupDetails.id == null) throw new ArgumentNullException(nameof(badgeGroupDetails.id), "Badge Group ID cannot be null");

            // check if any badge thresholds are met
            var badgeToAward = badgeGroupDetails.BadgeDetails
                                        .Where(b => (b as BadgeDetailsStreak).CountThreshold == gamerEventData.CurrentStreak) // find any badge with a threshold matching the current count
                                        .SingleOrDefault(); // should only be one

            // if no badges return null
            if (badgeToAward == null) return null;

            // does achievement already exist
            if (await _achievementsLogic.Exists(gamer.id.Value, badgeToAward.id.Value)) return null; // don't error just say we haven't earned anything here

            // if threshold met - return new badge
            return badgeToAward; 
        }
    }
}
