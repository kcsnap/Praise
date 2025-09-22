using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GamersController : Controller
    {
        /// <summary>
        /// add a new gamer to the authenticated users account and return the GamerID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Gamer> Post(NewGamer newGamer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// update an existing gamer to the authenticated users account and return the Gamer details
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{gamerId}")]
        public async Task<Gamer> Patch(string gamerId, NewGamer newGamer)
        {
            throw new NotImplementedException();
        }
                
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

        [HttpGet("{gamerId}")]
        public async Task<ResponseGamer> Get(string gamerId)
        {
            return new ResponseGamer
            {
                Gamer = new Gamer
                {
                    EmailAddress = "test@test.com",
                    Id = gamerId
                }
            };
        }
    }

    public class NewGamer
    {
        public string EmailAddress { get; set; }
    }

    public class Gamer : NewGamer
    {
        public string Id { get; set; }
    }

    public class ResponseGamer
    {
        public Gamer Gamer { get; set; }
    }
    public class ResponseGamers
    {
        public IEnumerable<Gamer> Gamers { get; set; }
    }

}
