using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Badges
{
    public class BadgeLogic(IGamerLogic _gamerLogic, IAchievementsLogic _achievementsLogic, IAchievementsFactory _achievementsFactory, IBadgeGroupDataLayer _badgeGroupDataLayer, IBadgeDataLayer _badgeDataLayer) : IBadgeLogic
    {
        
        public async Task<IEnumerable<BadgeGroupDetails>> GetBadgesAvailableForGame(Guid gameId)
        {
            var badgeGroups = await _badgeGroupDataLayer.Read(gameId);
            return badgeGroups;
        }


        // TODO - implement badges system
        // This could be a class that manages badges for users, including methods to award badges based on certain criteria.
        // For example, a badge for sending 1,2,5 (etc) praises.
        public async Task<IEnumerable<BaseBadgeDetails>> GetBadgesEarnedForEvent(Guid gameId, Guid gamerId, GameEvent gameEventCode)
        {
            BaseBadgeDetails badgeDetails;

            if (gameEventCode == null || !gameEventCode.id.HasValue) throw new Exception("gameEventCode cannot be null");

            // get badges associated to this event and gamer - event codes 
            // may be duplicated within a game so ensure looking at corrcet 
            // game by checking association with gamer
            // NB - may be more than one badge!
            var badgeGroups = await _badgeGroupDataLayer.ReadByEventCode(gameId, gameEventCode.id.Value);

            if (badgeGroups == null || badgeGroups.Count() == 0)
                return null;

            // check user exists
            var gamer = await _gamerLogic.GetGamerById(gamerId);
            if(gamer == null)
                throw new Exception("Gamer not found");

            // check badge logic based on badge type to see if we have earned a new badge
            var badgeGroupDetails = new List<BaseBadgeDetails>();
            foreach (var badgeGroup in badgeGroups)
            {
                var earnedBadge = await IsEarned(gamer, badgeGroup);
                if(earnedBadge != null) badgeGroupDetails.Add(earnedBadge);
            }

            // todo
            return badgeGroupDetails;
        }

        protected async Task<BaseBadgeDetails> IsEarned(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {
            if(gamer.id == null || badgeGroupDetails.id == null)
                throw new Exception("Error null gamerid: " + gamer.id + " and/or badgeGroup: " + badgeGroupDetails.id);

            // use factory to get correct badge type
            var badgeGroupLogic = _achievementsFactory.Get(badgeGroupDetails);

            // attempt awarding of badge...
            var badgeAwarded = await badgeGroupLogic.AwardBadge(gamer, badgeGroupDetails);

            // if no badge then nothing earned this time round
            if (badgeAwarded == null) return null;

            var achievement = await _achievementsLogic.AddAchievement(gamer.id.Value, badgeAwarded.id.Value);
            if (achievement == null) throw new Exception("Error adding achievement for gamerid: " + gamer.id + " and badgeGroup: " + badgeGroupDetails.id);

            return badgeAwarded;
        }
    }
}
