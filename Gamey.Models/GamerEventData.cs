namespace Gamey.Api.Models
{
    public class GamerEventData
    {
        public Guid? id { get; set; }
        public Guid GamerId { get; set; }
        public Guid GameEventId { get; set; }
        public int? Count { get; set; }
        public DateTime? LastEventDate { get; set; }
    }
}
