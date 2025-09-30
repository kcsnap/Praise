using System.Text.Json.Serialization;

namespace Gamey.Api.Models
{
    public class Gamer
    {
        public Guid? id { get; set; } // internal id
        public string GamerName { get; set; } // unique name for the gamer
        public string GamerEmail { get; set; } // unique email for the gamer

        // ref to game this user is associated with 
        [JsonIgnore]
        public Game Game { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }
    }
}
