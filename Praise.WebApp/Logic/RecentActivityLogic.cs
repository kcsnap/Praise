namespace Praise.WebApp.Logic
{
    public class RecentActivityLogic
    {
        /// <summary>
        /// retrieves recent activity specific to user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<RecentActivity> GetRecentActivityForUser(string userEmail)
        {
            // run separate tasks to return dashboard information
            var totalPraisesSentTask = GetTotalPraisesSentTask();
            var recentPraisesReceivedTask = GetRecentPraisesReceived();
            var recentPraisesSentTask = GetRecentPraisesSent();
            var recentBadgesReceivedTask = GetRecentBadgesReceived();

            await Task.WhenAll(totalPraisesSentTask, 
                               recentPraisesReceivedTask,
                               recentPraisesSentTask,
                               recentBadgesReceivedTask);

            return new RecentActivity()
            {
                TotalNumberOfPraisesSent = totalPraisesSentTask.Result,
                PraisesReceived = recentPraisesReceivedTask.Result,
                PraisesSent = recentPraisesSentTask.Result,
                BadgesReceived = recentBadgesReceivedTask.Result,
            };
        }

        /// <summary>
        /// calculate tally of number of praises sent
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetTotalPraisesSentTask()
        {
            return 10;
        }

        /// <summary>
        /// Returns max 3 praises received
        /// </summary>
        /// <returns></returns>
        private async Task<IList<PraiseReceived>> GetRecentPraisesReceived()
        {
            // TODO with DB call for current user
            return new List<PraiseReceived>()
            {
                new PraiseReceived
                {
                    From = "praiser1@praise.com",
                    ImageUrl = "badge-url.svg",
                    Text = "Thank you for all your hard work!"
                    
                },
                new PraiseReceived
                {
                    From = "praiser1@praise.com",
                    ImageUrl = "badge-url.svg",
                    Text = "Thank you for all your hard work!"
                },
                new PraiseReceived
                {
                    From = "praiser1@praise.com",
                    ImageUrl = "badge-url.svg",
                    Text = "Thank you for all your hard work!"
                }
            };
        }

        /// <summary>
        /// Returns max 3 praises sent
        /// </summary>
        /// <returns></returns>
        private async Task<IList<PraiseSent>> GetRecentPraisesSent()
        {
            // TODO with DB call for current user
            return new List<PraiseSent>()
            {
                new PraiseSent
                {
                    To = "praisee@praise.com",
                    ImageUrl = "badge-url.svg",
                    Text = "Thank you for all your hard work!"

                },
                new PraiseSent
                {
                    To = "praisee@praise.com",
                    ImageUrl = "badge-url.svg",
                    Text = "Thank you for all your hard work!"
                },
                new PraiseSent
                {
                    To = "praisee@praise.com",
                    ImageUrl = "badge-url.svg",
                    Text = "Thank you for all your hard work!"
                }
            };
        }

        /// <summary>
        /// Returns max 3 praises received
        /// </summary>
        /// <returns></returns>
        private async Task<IList<BadgeReceived>> GetRecentBadgesReceived()
        {
            // TODO with DB call for current user
            return new List<BadgeReceived>()
            {
                new BadgeReceived
                {
                    ImageUrl = "badge-url.svg",
                    Text = "Serial Praiser"
                },
                new BadgeReceived
                {
                    ImageUrl = "badge-url.svg",
                    Text = "Serial Helper"
                },
                new BadgeReceived
                {
                    ImageUrl = "badge-url.svg",
                    Text = "100 Day Streak"
                },
            };
        }
    }

    public class RecentActivity
    {
        public int? TotalNumberOfPraisesSent { get; set; }
        public IList<PraiseSent> PraisesSent { get; set; }
        public IList<PraiseReceived> PraisesReceived { get; set; }
        public IList<BadgeReceived> BadgesReceived { get; set; }
    }
    public abstract class BasePraise
    {
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PraiseDate { get; set; }
    }
    public class PraiseSent : BasePraise
    {
        public string To { get; set; }
    }
    public class PraiseReceived : BasePraise
    {
        public string From { get; set; }
    }
    public class BadgeReceived : BasePraise
    {

    }
}
