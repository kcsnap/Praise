using System.Text.Json.Serialization;

namespace Gamey.Api.Models
{
    public class Game
    {
        public Guid? id { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        [JsonIgnore]
        public string ClientId { get; set; }
        [JsonIgnore]
        public string EncryptedApiKey { get; set; }
        [JsonIgnore]
        public IList<BadgeGroupDetails> BadgeGroups{ get; set; }
    }
}
