using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]    
    public class GameEventController(IGameLogic _gameLogic, IGamerLogic _gamerLogic, IGameEventsLogic _gameEventsLogic, IBadgeLogic _badgeLogic, IPrizeGroupLogic _prizeLogic) : Controller
    {
        [HttpGet("gameevent")]
        public async Task<IEnumerable<GameEvent>> Get()
        {
            var game = await _gameLogic.GetGame();
            if (game == null) throw new Exception("Error retrieving game");

            var events = await _gameEventsLogic.GetGameEvents(game.id.Value);
            return events;
        }

        /// <summary>
        /// Indicate the user has triggered a new event and return NEW (only!) badges awarded as a result
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
            var gameEvent = await _gameEventsLogic.GetGameEventByCode(newGameEvent.EventCode);
            if (gameEvent == null) throw new BadHttpRequestException("EventCode is invalid");

            var game = await _gameLogic.GetGame();
            if (game == null || !game.id.HasValue) throw new BadHttpRequestException("game not found");

            var gamer = await _gamerLogic.GetGamerByEmail(newGameEvent.GamerEmail);
            if (gamer == null || !gamer.id.HasValue) throw new BadHttpRequestException("gamer not found");

            // record event and then check for new badges and prizes...
            var badges = await _badgeLogic.GetBadgesEarnedForEvent(game.id.Value, gamer.id.Value, gameEvent);
            var prize = await _prizeLogic.GetPrizeEarnedForEvent(game.id.Value, gamer.id.Value, gameEvent);

            return new ResponseBadges
            {
                EarnedBadges = badges,
                EarnedPrize = prize
            };
        }
    }
}
