using Gamey.Models;

namespace Gamey.Api.Models.Responses
{
    public class ResponseBadges
    {
        public IEnumerable<BaseBadgeDetails> EarnedBadges { get; set; }
        public PrizeGroups EarnedPrize { get; set; }
    }
}
