using Gamey.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gamey.Models.Converters
{
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
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
