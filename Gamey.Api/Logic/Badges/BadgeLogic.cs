using Gamey.Api.Interfaces;
using Gamey.Api.Logic.Achievements;
using Gamey.Api.Models;

namespace Gamey.Api.Logic.Badges
{
    public class BadgeLogic : IBadgeLogic
    {
        private readonly IGamerLogic _gamerLogic;
        private readonly IAchievementsLogic _achievementsLogic;
        private readonly IAchievementsFactory _achievementsFactory;

        protected IList<BadgeGroupDetails> BadgeGroups { get; private set; }

        public BadgeLogic(IGamerLogic gamerLogic, IAchievementsLogic achievementsLogic, IAchievementsFactory achievementsFactory)
        {
            _gamerLogic = gamerLogic;
            _achievementsLogic = achievementsLogic;
            _achievementsFactory = achievementsFactory;

            // TO REMOVE! Replace with DB!
            LoadBadges();
        }

        /// <summary>
        /// todo - driven by database for user, dummy data for Praise app
        /// </summary>
        /// <returns></returns>
        protected void LoadBadges()
        {
            // Load badges from database or configuration
            BadgeGroups = new List<BadgeGroupDetails>
            {
                new BadgeGroupDetails
                {
                    Id = "badgegroup-special-registration",
                    BadgeTitle = "Welcome Badge",
                    BadgeDescription = "Awarded for registering with the game",
                    BadgeType = BadgeType.Special,
                    GameEventCodeId = new Guid("a3fa083d-d1c8-4d63-a047-00a7d0bd67ca"),
                    GameId = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210"),
                    // GameEventCode = provides link to instance based on id
                    BadgeDetails = new List<BaseBadgeDetails>
                    {
                        new BadgeDetailsSpecial() {
                            Id = "badge-special-register",
                            BadgeImageUrl= "todo",
                            BadgeSubtitle = "Welcome Aboard!",
                        }
                    }
                },
                new BadgeGroupDetails
                {
                    Id = "badgegroup-count-praise",
                    BadgeTitle = "Praiser Badge",
                    BadgeDescription = "Awarded for sending praise within the game",
                    BadgeType = BadgeType.Count,
                    GameEventCodeId = new Guid("df06eeaa-2e1f-4342-afca-20ab0624e830"),
                    GameId = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210"),
                    BadgeDetails = new List<BaseBadgeDetails>
                    {
                        new BadgeDetailsCount
                        {
                            Id = "badge-count-send-praise-1",
                            BadgeSubtitle = "First Praising!",
                            CountThreshold = 1,
                            BadgeImageUrl = "todo"
                        },
                        new BadgeDetailsCount
                        {
                            Id = "badge-count-send-praise-2",
                            BadgeSubtitle = "Second Praising!",
                            CountThreshold = 2,
                        },
                        new BadgeDetailsCount
                        {
                            Id = "badge-count-send-praise-3",
                            BadgeSubtitle = "Third Praising!",
                            CountThreshold = 3,
                        },
                        new BadgeDetailsCount
                        {
                            Id = "badge-count-send-praise-5",
                            BadgeSubtitle = "Fifth Praising!",
                            CountThreshold = 5,
                        },
                    }
                },
                new BadgeGroupDetails
                {
                    Id = "badgegroup-streak-login",
                    BadgeTitle = "Frequent Visitor Badge",
                    BadgeDescription = "Awarded for logging in consecutive days",
                    BadgeType = BadgeType.Streak,
                    GameEventCodeId = new Guid("52705840-35e3-420a-b7df-8db2e856d9f9"), // matched with id on GameEventCode class
                    GameId = new Guid("01234567-89AB-CDEF-FEDC-BA9876543210"),
                    BadgeDetails = new List<BaseBadgeDetails>
                    {
                        new BadgeDetailsStreak
                        {
                            Id = "badge-streak-login-2",
                            BadgeSubtitle = "Welcome back!",
                            CountThreshold = 2,
                            BadgeImageUrl = "todo"
                        },
                        new BadgeDetailsStreak
                        {
                            Id = "badge-streak-login-3",
                            BadgeSubtitle = "Third login!",
                            CountThreshold = 3,
                            BadgeImageUrl = "todo"
                        },
                        new BadgeDetailsStreak
                        {
                            Id = "badge-streak-login-4",
                            BadgeSubtitle = "Fourth times a charm!",
                            CountThreshold = 4,
                            BadgeImageUrl = "todo"
                        },
                        new BadgeDetailsStreak
                        {
                            Id = "badge-streak-login-5",
                            BadgeSubtitle = "Lucky nuber 5!",
                            CountThreshold = 5,
                            BadgeImageUrl = "todo"
                        },
                        new BadgeDetailsStreak
                        {
                            Id = "badge-count-send-praise-5",
                            BadgeSubtitle = "Fifth Praising!",
                            CountThreshold = 4,
                            BadgeImageUrl = "todo"
                        },
                    }
                }
            };
        }

        public async Task<IEnumerable<BadgeGroupDetails>> GetBadgesAvailableForGame(Guid gameId)
        {
            // get badges associated to this event and gamer - event codes 
            // may be duplicated within a game so ensure looking at corrcet
            // game by checking association with gamer
            // NB - may be more than one badge!
            var badgeGroups = BadgeGroups.Where(e => e.GameId == gameId);

            if (badgeGroups == null || badgeGroups.Count() == 0)
                return null;

            // todo
            return badgeGroups.ToList();
        }


        // TODO - implement badges system
        // This could be a class that manages badges for users, including methods to award badges based on certain criteria.
        // For example, a badge for sending 1,2,5 (etc) praises.
        public async Task<IEnumerable<BaseBadgeDetails>> GetBadgesEarnedForEvent(string gamerId, GameEvent gameEventCode)
        {
            BaseBadgeDetails badgeDetails;

            // get badges associated to this event and gamer - event codes 
            // may be duplicated within a game so ensure looking at corrcet 
            // game by checking association with gamer
            // NB - may be more than one badge!
            var badgeGroups = BadgeGroups.Where(e => e.GameEventCodeId == gameEventCode.id);

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
                badgeGroupDetails.Add(await IsEarned(gamer, badgeGroup));
            }

            // todo
            return badgeGroupDetails;
        }

        protected async Task<BaseBadgeDetails> IsEarned(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {
            // use factory to get correct badge type
            var badgeGroupLogic = _achievementsFactory.Get(badgeGroupDetails);

            // attempt awarding of badge...
            var badgeAwarded = await badgeGroupLogic.AwardBadge(gamer, badgeGroupDetails);

            await _achievementsLogic.SaveAchievement(new Achievement
            {
                Id = Guid.NewGuid().ToString(),
                BadgeId = badgeAwarded.Id,
                GamerId = gamer.Id,
                DateAchieved = DateTime.UtcNow
            });

            return badgeAwarded;
        }
    }
}
