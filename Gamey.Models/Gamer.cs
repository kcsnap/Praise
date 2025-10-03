using System.Text.Json.Serialization;

namespace Gamey.Api.Models
{
    public class NewGamer
    {
        public string GamerName { get; set; } // unique name for the gamer
        public string GamerEmail { get; set; } // unique email for the gamer
    }

    public class Gamer : NewGamer
    {
        public Guid? id { get; set; } // internal id
        
        // ref to game this user is associated with 
        [JsonIgnore]
        public Game Game { get; set; }
        [JsonIgnore]
        public Guid GameId { get; set; }
    }
}
