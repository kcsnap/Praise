using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    public class GamerController : Controller
    {
        /// <summary>
        /// add a new gamer to the authenticated users account and return the GamerID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<Gamer> Post(NewGamer newGamer)
        {
            throw new NotImplementedException();
        }
    }

    public class NewGamer
    {
        public string EmailAddress { get; set; }
    }

    public class Gamer : NewGamer
    {
        public int Id { get; set; }
    }

}
