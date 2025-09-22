using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class GameEventController(IGameEventsLogic gameEventsLogic, IBadgeLogic badgeLogic) : Controller
    {
        /// <summary>
        /// Indiciate the user has triggered a new event and return NEW (only!) badges awarded as a result
        /// </summary>
        /// <param name="newGameEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ResponseBadge> Post(NewGameEvent newGameEvent)
        {
            if(newGameEvent == null || string.IsNullOrEmpty(newGameEvent.EventCode))
                throw new BadHttpRequestException("EventCode is mandatory");

            // find event by code
            var gameEvent = await gameEventsLogic.GetGameEventByCode(newGameEvent.EventCode);

            if (gameEvent == null)
                throw new BadHttpRequestException("EventCode is invalid");

            // check for new badges...
            // TODO RETURN NEW BADGES, IF ANY!!!
            await badgeLogic.CheckBadges("123", gameEvent);

            return new ResponseBadge
            {
                Badges = new List<BadgeGroupDetails>()
            };
        }
    }
}
