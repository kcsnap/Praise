using Microsoft.AspNetCore.Mvc;
using Praise.Models;
using Praise.WebApp.Logic;
using Microsoft.AspNetCore.Authorization;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PraiseController(ILogger<PraiseController> _logger, IHttpContextAccessor _contextAccessor, IPraiseLogic praiseLogic)
    {
        /// <summary>
        /// /praise?type=sent | received
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>        
        [HttpGet]
        public async Task<IEnumerable<PraiseDetails>> Get([FromQuery(Name = "issender")] bool isSender, [FromQuery(Name = "pagenumber")] int? pageNumber, [FromQuery(Name = "numberperpage")] int? numberPerPage)
        {
            IEnumerable<PraiseDetails> praises;

            var email = _contextAccessor.HttpContext.Request.Headers["username"];

            if (string.IsNullOrEmpty(email))
                throw new BadHttpRequestException("Email is mandatory");

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
