namespace Gamey.Api.Models
{
    public class Achievement
    {
        public Guid? id { get; set; }
        public Guid GamerId { get; set; } // foreign key to Gamer
        public Guid BadgeId { get; set; } // foreign key to Badge
        public DateTime DateAchieved { get; set; }
        public DateTime? DateClaimed { get; set; } // game must 'claim' the achievement to ensure user is presented with new badge
    }
}
