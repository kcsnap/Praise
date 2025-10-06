using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic.Interfaces;
using Praise.Models;
using Praise.WebApp.Logic;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DashboardCountsController(ILogger<DashboardCountsController> _logger, IPrizeLogic _prizeLogic, IPraiseUserLogic _praiseUserLogic, IPraiseLogic _praiseLogic) : Controller
    {
        [HttpGet(Name = "Retrieve Dashboard Counts")]
        public async Task<DashboardViewModel> Get()
        {
            try
            {
                var cryptoBalance = await _prizeLogic.GetCryptoBalance(_praiseUserLogic);
                var received = await _praiseLogic.GetPraiseDetails(_praiseUserLogic.PraiseUser.Email, false, 1, 100); // max 100
                var sent = await _praiseLogic.GetPraiseDetails(_praiseUserLogic.PraiseUser.Email, true, 1, 100); // max 100

                return new DashboardViewModel
                {
                    Balance = cryptoBalance,
                    Sent = sent.Count(),
                    Received = received.Count(),
                };
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
