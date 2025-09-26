using Gamey.Api.Interfaces;
using Gamey.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml;

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
        /// <exception cref="NotImplementedException"></exception>        
        [HttpGet("badges")]
        public async Task<string> Get()
        {
            // we don't need user here, we just need clientid from authenticated client
            var clientId = _contextAccessor.HttpContext.Request.Headers["X-CLIENT-ID"];
            var game = await _gameLogic.GetGame(clientId);

            if (game == null) throw new Exception("Error retrieving game for client id : " + clientId);

            // use gameid to get all badges for that game
            var badges = await _badgeLogic.GetBadgesAvailableForGame(game.id.Value);

            // due to inability of system.json convert we have to use newtonsoft here...
            string jsonTypeNameAll = JsonConvert.SerializeObject(badges, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None // remove all type information returned to user
            });

            return jsonTypeNameAll;
        }
    }
}
