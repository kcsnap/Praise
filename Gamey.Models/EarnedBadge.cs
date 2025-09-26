namespace Gamey.Api.Models
{
    public class EarnedBadge : BadgeGroupDetails
    {
        public BaseBadgeDetails BadgeDetails { get; set; }
        public bool IsBadgeEarned { get; set; }
        public bool IsBadgeClaimed { get; set; }
        public DateTime? BadgeEarnedDate { get; set; }
        public DateTime? BadgeClaimedDate { get; set; }
    }
}
