namespace Gamey.Api.Models
{
    public class Achievement
    {
        public string Id { get; set; }
        public string GamerId { get; set; } // foreign key to Gamer
        public string BadgeId { get; set; } // foreign key to Badge
        public DateTime DateAchieved { get; set; }
    }
}
