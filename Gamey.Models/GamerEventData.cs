namespace Gamey.Api.Models
{
    public class GamerEventData
    {
        public Guid? id { get; set; }
        public Guid GamerId { get; set; }
        public Guid GameEventId { get; set; }
        public Guid? BadgeGroupDetailsId { get; set; }
        public Guid? PrizeGroupDetailsId { get; set; }
        public int? Count { get; set; }
        public int? CurrentStreak { get; set; }
        public int? MaxStreak { get; set; }
        public DateTime LastEventDate { get; set; }
    }
}
