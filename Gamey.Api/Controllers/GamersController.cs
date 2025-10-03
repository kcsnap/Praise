using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [Authorize]
    [ApiController]    
    public class GamersController(IGamerLogic _gamerLogic) : Controller
    {
        /// <summary>
        /// add a new gamer to the authenticated users account and return the GamerID
        /// </summary>
        /// <returns></returns>
        [HttpPost("gamers")]
        public async Task<Gamer> Post(NewGamer newGamer)
        {
            var gamer = await _gamerLogic.AddGamer(newGamer);
            return gamer;
        }

        [HttpGet("gamers/{gamerEmail}")]
        public async Task<Gamer> Get(string gamerEmail)
        {
            return await _gamerLogic.GetGamerByEmail(gamerEmail);
        }
    }
}
