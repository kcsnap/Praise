using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AchievementsController(IHttpContextAccessor _contextAccessor, IAchievementsLogic _achievementsLogic) : Controller
    {
        /// <summary>
        /// Return a list of ALL available badges indicating if the authenticated user has earned them or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>        
        [HttpGet("gamers/{gamerId}/achievements")]
        public async Task<IEnumerable<Achievement>> Get(string gamerId)
        {
            // use gameid to get all badges for that game
            var achievements = await _achievementsLogic.GetAchievementsForGamer(gamerId);
            return achievements;
        }

        [HttpPatch("gamers/{gamerId}/achievements/{achievementId}")]
        public async Task<Achievement> Claim(string gamerId, string achievementId)
        {
            // use gameid to get all badges for that game
            var achievement = await _achievementsLogic.UpdateAchievement(gamerId, achievementId);
            return achievement;
        }
    }
}
