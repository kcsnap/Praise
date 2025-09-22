using Gamey.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BadgesController : Controller
    {
        /// <summary>
        /// Return a list of ALL available badges indicating if the authenticated user has earned them or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{gamerId}")]
        public async Task<ResponseEarnedBadges> Get(string gamerId)
        {
            throw new NotImplementedException();
        }
    }
}
