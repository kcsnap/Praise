namespace Praise.Models
{
    public class RecentActivity
    {
        public int? TotalNumberOfPraisesSent { get; set; }
        public IList<PraiseSent> PraisesSent { get; set; }
        public IList<PraiseReceived> PraisesReceived { get; set; }
        public IList<BadgeReceived> BadgesReceived { get; set; }
    }
    public abstract class BasePraise
    {
        public PraiseTypeEnum PraiseType { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? PraiseDate { get; set; }
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
