using Microsoft.AspNetCore.Mvc;
using Praise.Models;
using Praise.WebApp.Logic;
using Microsoft.AspNetCore.Authorization;
using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PraiseController(ILogger<PraiseController> _logger, IHttpContextAccessor _contextAccessor, IPraiseLogic praiseLogic, IGameWrapperLogic gameLogic, IPraiseUserLogic _praiseUserLogic)
    {
        /// <summary>
        /// /praise?type=sent | received
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>        
        [HttpGet]
        public async Task<IEnumerable<PraiseDetails>> Get([FromQuery(Name = "issender")] bool isSender, [FromQuery(Name = "pagenumber")] int? pageNumber, [FromQuery(Name = "numberperpage")] int? numberPerPage)
        {
            if (_praiseUserLogic.PraiseUser == null || string.IsNullOrEmpty(_praiseUserLogic.PraiseUser.Email)) throw new Exception("Error with loggedin user");
            var email = _praiseUserLogic.PraiseUser.Email;
            
            IEnumerable<PraiseDetails> praises;
            praises = await praiseLogic.GetPraiseDetails(email, isSender, pageNumber, numberPerPage);

            return praises;
        }

        [HttpPost(Name = "PostPraise")]
        public async Task<PraiseDetails> Post(PraiseDetails praiseSent)
        {
            try
            {
                var email = _contextAccessor.HttpContext.Request.Headers["username"];

                if (string.IsNullOrEmpty(email))
                    throw new BadHttpRequestException("Email is mandatory");

                praiseLogic.PublishSendPraiseEvent(praiseSent);

                return praiseSent;
            }
            catch (Exception ex)
            {
                var msg = "PostPraise() Error: [See inner exception]";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
    }
}
