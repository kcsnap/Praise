namespace Gamey.Api.Models
{
    public class BadgeGroupDetails
    {
        public string Id { get; set; }
        public string BadgeTitle { get; set; }
        public string BadgeDescription { get; set; }
        public BadgeType BadgeType { get; set; }
        public IList<BaseBadgeDetails> BadgeDetails { get; set; }

        // ref to master game these badges are associated to
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Game Game { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid GameId { get; set; }

        // ref to game event code that this badge group is associated with
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public GameEvent GameEventCode { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid GameEventCodeId { get; set; }
    }

    public abstract class BaseBadgeDetails
    {
        public string Id { get; set; }
        public string BadgeSubtitle { get; set; }
        public string BadgeImageUrl { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public BadgeGroupDetails BadgeGroupDetails { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string BadgeGroupDetailsId { get; set; }
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
