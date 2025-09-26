using Gamey.Api.Interfaces;
using Gamey.Api.Logic;
using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]    
    public class GameEventController(IHttpContextAccessor _contextAccessor, IGameLogic _gameLogic, IGameEventsLogic gameEventsLogic, IBadgeLogic badgeLogic) : Controller
    {
        [HttpGet("gameevent")]
        public async Task<IEnumerable<GameEvent>> Get()
        {
            // we don't need user here, we just need clientid from authenticated client
            var clientId = _contextAccessor.HttpContext.Request.Headers["X-CLIENT-ID"];
            var game = await _gameLogic.GetGame(clientId);

            if (game == null) throw new Exception("Error retrieving game for client id : " + clientId);

            var events = await gameEventsLogic.GetGameEvents(game.id.Value);
            return events;
        }

        /// <summary>
        /// Indiciate the user has triggered a new event and return NEW (only!) badges awarded as a result
        /// </summary>
        /// <param name="newGameEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("gameevent")]        
        public async Task<ResponseBadges> Post(NewGameEvent newGameEvent)
        {
            if(newGameEvent == null || string.IsNullOrEmpty(newGameEvent.EventCode))
                throw new BadHttpRequestException("EventCode is mandatory");

            // find event by code
            var gameEvent = await gameEventsLogic.GetGameEventByCode(newGameEvent.EventCode);

            if (gameEvent == null)
                throw new BadHttpRequestException("EventCode is invalid");

            // check for new badges...
            // TODO RETURN NEW BADGES, IF ANY!!!
            var badges = await badgeLogic.GetBadgesEarnedForEvent(newGameEvent.GamerId, gameEvent);

            return new ResponseBadges
            {
                EarnedBadges = badges
            };
        }
    }
}
