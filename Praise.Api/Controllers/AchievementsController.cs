using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AchievementsController(ILogger<PraiseController> _logger, IGameWrapperLogic _gameLogic, IPraiseUserLogic _praiseUserLogic) : Controller
    {
        [HttpPost(Name = "Retrieve all achievements")]
        public async Task Get()
        {
            try
            {
                await _gameLogic.GetAchievements(_praiseUserLogic);
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
