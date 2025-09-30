using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class BadgesController(IHttpContextAccessor _contextAccessor, IGameLogic _gameLogic, IBadgeLogic _badgeLogic) : Controller
    {
        /// <summary>
        /// Return a list of ALL available badges indicating if the authenticated user has earned them or not.
        /// </summary>
        /// <returns></returns>        
        [HttpGet("badges")]
        /*public async Task<string> Get()*/
        public async Task<IEnumerable<BadgeGroupDetails>> Get()
        {
            var game = await _gameLogic.GetGame();

            if (game == null) throw new Exception("Error retrieving game");

            // use gameid to get all badges for that game
            var badges = await _badgeLogic.GetBadgesAvailableForGame(game.id.Value);

            return badges;
        }
    }
}
