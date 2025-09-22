namespace Gamey.Api.Models
{
    public class EarnedBadge : BadgeGroupDetails
    {
        public bool IsBadgeEarned { get; set; }
        public DateTime BadgeEarnedDate { get; set; }
    }
}
