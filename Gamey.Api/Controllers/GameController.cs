using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class GameController(IGameLogic _gameLogic) : Controller
    {
        /// <summary>
        /// Return a list of Game authenticated to use.
        /// </summary>
        /// <returns></returns>        
        [HttpGet("game")]
        public async Task<Game> Get()
        {
            var game = await _gameLogic.GetGame();

            if (game == null) throw new Exception("Error retrieving game");

            return game;
        }
    }
}
