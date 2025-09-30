namespace Praise.Models
{
    public class BadgeGroupViewModel
    {
        public Guid BadgegroupId { get; set; }
        public string BadgeTitle { get; set; }
        public IList<BadgeDetailsViewModel> BadgeDetails { get; set; }
    }
    public class BadgeDetailsViewModel
    {
        public Guid BadgeId { get; set; }
        public string BadgeSubtitle { get; set; }
        public string BadgeImageUrl { get; set; }
        public DateTime? BadgeDateEarned { get; set; }
        public DateTime? BadgeDateClaimed { get; set; }
    }
}
