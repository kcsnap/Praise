using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    public class BadgesController : Controller
    {
        /// <summary>
        /// Return a list of ALL available badges indicating if the authenticated user has earned them or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<EarnedBadge>> Get(Gamer gamer)
        {
            // requires both id and email to return 

            throw new NotImplementedException();
        }
    }

    public class Badge
    {
        public int Id { get; set; }
        public string BadgeTitle { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeUrl { get; set; }
    }

    public class EarnedBadge : Badge
    {
        public bool IsBadgeEarned { get; set; }
        public DateTime BadgeEarnedDate { get; set; }
    }
}
