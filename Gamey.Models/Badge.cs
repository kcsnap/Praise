using Gamey.Models.Converters;
using System.Text.Json.Serialization;

namespace Gamey.Api.Models
{

    [JsonConverter(typeof(BadgeDetailsConverter))]
    public abstract class BaseBadgeDetails
    {
        public Guid? id { get; set; }
        public string BadgeSubtitle { get; set; }
        public string BadgeImageUrl { get; set; }
        public BadgeType BadgeType { get; set; }

        [JsonIgnore]
        public BadgeGroupDetails BadgeGroupDetails { get; set; }
        [JsonIgnore]
        public string BadgeGroupDetailsId { get; set; }
    }

    public class BadgeDetailsSpecial : BaseBadgeDetails
    {
    }

    public class BadgeDetailsCount : BaseBadgeDetails
    {
        public int CountThreshold { get; set; }
    }

    public class BadgeDetailsStreak : BaseBadgeDetails
    {
        public int CountThreshold { get; set; }
    }
}
