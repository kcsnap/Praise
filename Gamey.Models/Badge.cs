using System.Text.Json;
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

    public class BadgeDetailsConverter : JsonConverter<BaseBadgeDetails>
    {
        public BadgeDetailsConverter() { }

        public override BaseBadgeDetails? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Parse JSON object
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var obj = jsonDoc.RootElement;

            // Determine badge type from some property in JSON
            // (You need a property to distinguish badge types, e.g., "badgeType")
            if (!obj.TryGetProperty("BadgeType", out var badgeTypeProperty))
            {
                if(!obj.TryGetProperty("badgeType", out badgeTypeProperty))
                    throw new Exception("Badge type not found in JSON");
            }
                

            string badgeType = badgeTypeProperty.GetString() ?? throw new Exception("badgeType is null");

            return badgeType switch
            {
                "Streak" => JsonSerializer.Deserialize<BadgeDetailsStreak>(obj.GetRawText(), options),
                "Special" => JsonSerializer.Deserialize<BadgeDetailsSpecial>(obj.GetRawText(), options),
                "Count" => JsonSerializer.Deserialize<BadgeDetailsCount>(obj.GetRawText(), options),
                _ => throw new Exception("Badge type not recognised")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseBadgeDetails value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}
