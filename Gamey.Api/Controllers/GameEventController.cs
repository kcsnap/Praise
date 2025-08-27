using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    public class GameEventController : Controller
    {
        /// <summary>
        /// Indiciate the user has triggered a new event and return NEW (only!) badges awarded as a result
        /// </summary>
        /// <param name="newGameEvent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Authorize]
        public async Task<IEnumerable<Badge>> Index(NewGameEvent newGameEvent)
        {


            throw new NotImplementedException();
        }
    }

    public class NewGameEvent
    {
        public string EmailAddress { get; set; }
        public GameEventCode EventIdentifier { get; set; }
    }

    public class GameEventCode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }


}
