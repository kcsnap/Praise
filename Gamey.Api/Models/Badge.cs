namespace Gamey.Api.Models
{
    public class BadgeGroupDetails
    {
        public string Id { get; set; }
        public string BadgeTitle { get; set; }
        public string BadgeDescription { get; set; }
        public BadgeType BadgeType { get; set; }
        public IList<BaseBadgeDetails> BadgeDetails { get; set; }
        public GameEventCode GameEventCode { get; set; }
        public string GameEventCodeId { get; set; }
    }

    public abstract class BaseBadgeDetails
    {
        public string Id { get; set; }
        public string BadgeSubtitle { get; set; }
        public string BadgeImageUrl { get; set; }        
    }

    public class BadgeDetailsSpecial : BaseBadgeDetails
    {
    }

    public class BadgeDetailsCount : BaseBadgeDetails
    {
        public string BadgeSubtitle { get; set; }
        public int CountThreshold { get; set; }
    }

    public class BadgeDetailsStreak : BaseBadgeDetails
    {
        public string BadgeSubtitle { get; set; }
        public int CountThreshold { get; set; }
    }
}
