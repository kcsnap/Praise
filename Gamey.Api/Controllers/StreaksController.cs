using Gamey.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class StreaksController : Controller
    {
        [HttpGet("gamers/{gamerId}/streaks")]
        public async Task<ResponseStreak> Get()
        {
            return new ResponseStreak
            {
                CurrentStreak = 0,
                LastActiveDate = DateTime.Now,
                LongestStreak = 0
            };
        }
    }
}
