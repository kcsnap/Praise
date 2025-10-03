using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class AchievementsController(ILogger<PraiseController> _logger, IGameWrapperLogic _gameLogic, IPraiseUserLogic _praiseUserLogic) : Controller
    {
        [HttpPatch("achievements/{achievementId}")]
        public async Task Patch(Guid gamerId, Guid achievementId)
        {
            try
            {
                await _gameLogic.ClaimAchievement(_praiseUserLogic, achievementId);
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
