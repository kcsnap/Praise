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
        public async Task<Gamer> Post(Gamer gamer)
        {
            return await _gamerLogic.AddGamer(gamer);
        }

        ///// <summary>
        ///// update an existing gamer to the authenticated users account and return the Gamer details
        ///// </summary>
        ///// <returns></returns>
        //[HttpPatch("gamers/{gamerEmail}")]
        //public async Task<Gamer> Patch(string gamerEmail, Gamer gamer)
        //{
        //    return await _gamerLogic.UpdateGamer(gamerEmail, gamer);
        //}
                
        //[HttpGet]        
        //public async Task<GamersResponse> Get()
        //{
        //    return new GamersResponse
        //    {
        //        Gamers = new List<Gamer>()
        //        {
        //            new Gamer
        //            {
        //                EmailAddress = "test@test.com",
        //                Id = "123"
        //            },
        //            new Gamer
        //            {
        //                EmailAddress = "test123@test.com",
        //                Id = "456"
        //            }
        //        }
        //    };
        //}

        [HttpGet("gamers/{gamerEmail}")]
        public async Task<Gamer> Get(string gamerEmail)
        {
            return await _gamerLogic.GetGamerByEmail(gamerEmail);
        }
    }
}
