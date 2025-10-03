using System.Text.Json.Serialization;

namespace Gamey.Api.Models
{
    public class BadgeGroupDetails
    {
        public Guid? id { get; set; }
        public string BadgeTitle { get; set; }
        public string BadgeDescription { get; set; }
        public BadgeType BadgeType { get; set; }
        public IList<BaseBadgeDetails> BadgeDetails { get; set; }

        // ref to master game these badges are associated to
        [JsonIgnore]
        public Game Game { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }

        // ref to game event code that this badge group is associated with
        [JsonIgnore]
        public GameEvent GameEventCode { get; set; }
        [JsonIgnore]
        public Guid GameEventCodeId { get; set; }
    }
}
