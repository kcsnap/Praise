using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GameController(ILogger<GameController> _logger, IGameLogic _gameLogic, IPraiseUserLogic _praiseUserLogic)
    {
        [HttpPost(Name = "Register game has started")]
        public async Task Post()
        {
            try
            {
                await _gameLogic.RegisterGamer(_praiseUserLogic);
            }
            catch (Exception ex)
            {
                var msg = "PostGame() Error: [See inner exception]";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
    }
}
