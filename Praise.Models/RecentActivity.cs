namespace Praise.Models
{
    public class RecentActivity
    {
        public int? TotalNumberOfPraisesSent { get; set; }
        public IEnumerable<PraiseDetails> PraisesSent { get; set; }
        public IEnumerable<PraiseDetails> PraisesReceived { get; set; }
        public IList<BadgeDetailsViewModel> BadgesReceived { get; set; }
    }
}
