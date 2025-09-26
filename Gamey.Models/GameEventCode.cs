using System.Text.Json.Serialization;

namespace Gamey.Api.Models
{
    public class GameEvent
    {
        [JsonIgnore]
        public Guid id { get; set; } // this is used internally
        public string Name { get; set; }
        public string Code { get; set; } // this should be used- by the gamer to declare an event
        public string Description { get; set; }
        
        [JsonIgnore]
        public Guid GameId { get; set; }
    }
}
