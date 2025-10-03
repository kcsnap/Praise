using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PrizesController(ILogger<PrizesController> _logger, IPrizeLogic _prizeLogic, IPraiseUserLogic _praiseUserLogic) : Controller
    {
        [HttpGet(Name = "Retrieve Prize")]
        public async Task<int> Get()
        {
            try
            {
                return await _prizeLogic.GetCryptoBalance(_praiseUserLogic);
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
