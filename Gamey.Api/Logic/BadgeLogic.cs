using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class BadgeLogic : IBadgeLogic
    {
        private readonly IGamerLogic _gamerLogic;

        protected IList<BadgeGroupDetails> BadgeGroups { get; private set; }

        public BadgeLogic(IGamerLogic gamerLogic)
        {
            _gamerLogic = gamerLogic;
            Task.Run(LoadBadges);
        }

        /// <summary>
        /// todo - driven by database for user, dummy data for Praise app
        /// </summary>
        /// <returns></returns>
        protected async Task LoadBadges()
        {
            // Load badges from database or configuration
            BadgeGroups = new List<BadgeGroupDetails>
            {
                new BadgeGroupDetails
                {
                    Id = "badge-special-registration",
                    BadgeTitle = "Welcome Badge",
                    BadgeDescription = "Awarded for registering with the game",
                    BadgeType = BadgeType.Special,
                    GameEventCodeId = "gamer-registered",
                    // GameEventCode = provides link to instance based on id
                    BadgeDetails = (IList<BaseBadgeDetails>)new List<BadgeDetailsSpecial>
                    {
                        new() {
                            BadgeImageUrl= "todo",
                            BadgeSubtitle = "Welcome Aboard!",
                        }
                    }
                },
                new BadgeGroupDetails
                {
                    Id = "badge-count-praise",
                    BadgeTitle = "Praiser Badge",
                    BadgeDescription = "Awarded for sending praise within the game",
                    BadgeType = BadgeType.Count,
                    GameEventCodeId = "gamer-send-praise",
                    BadgeDetails = (IList<BaseBadgeDetails>)new List<BadgeDetailsCount>
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
                    Id = "badge-streak-login",
                    BadgeTitle = "Frequent Visitor Badge",
                    BadgeDescription = "Awarded for logging in consecutive days",
                    BadgeType = BadgeType.Streak,
                    GameEventCodeId = "gamer-login", // matched with id on GameEventCode class
                    BadgeDetails = (IList<BaseBadgeDetails>)new List<BadgeDetailsStreak>
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

        // TODO - implement badges system
        // This could be a class that manages badges for users, including methods to award badges based on certain criteria.
        // For example, a badge for sending 1,2,5 (etc) praises.
        public async Task<IList<BadgeGroupDetails>> CheckBadges(string gamerId, GameEventCode gameEventCode)
        {
            BaseBadgeDetails badgeDetails;

            // get badges associated to this event and gamer - event codes 
            // may be duplicated within a game so ensure looking at corrcet 
            // game by checking association with gamer
            // NB - may be more than one badge!
            var badgeGroups = BadgeGroups.Where(e => e.GameEventCodeId == gameEventCode.Id);

            if (badgeGroups == null || badgeGroups.Count() == 0)
                return null;

            // check user exists
            var gamer = await _gamerLogic.GetGamerByUniqueName(gamerId);
            if(gamer == null)
                throw new Exception("Gamer not found");

            // check badge logic based on badge type to see if we have earned a new badge
            foreach (var badgeGroup in badgeGroups)
            {
                var badgeGroupDetails = IsEarned(gamer, badgeGroup);
            }

            // todo
            return null;
        }

        // TODO - create factory for this...
        protected BaseBadgeDetails IsEarned(Gamer gamer, BadgeGroupDetails badgeGroupDetails)
        {            
            // if badge type special then award once only
            if (badgeGroupDetails.BadgeType == BadgeType.Special)
            {
                // check if already awarded

            }

            // todo
            return null;
        }
    }
}
