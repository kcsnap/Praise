using Microsoft.AspNetCore.Mvc;
using Praise.Models;
using Praise.WebApp.Logic;
using WebHttpPostAttribute = System.Web.Http;
using MvcHttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System.Web.Http;
using Microsoft.VisualBasic;

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
        [WebHttpPostAttribute.HttpGet()]
        public async Task<IEnumerable<PraiseDetails>> Get([FromQuery(Name = "issender")] bool isSender, [FromQuery(Name = "pagenumber")] int? pageNumber, [FromQuery(Name = "numberperpage")] int? numberPerPage)
        {
            IEnumerable<PraiseDetails> praises;

            var email = _contextAccessor.HttpContext.Request.Headers["username"];

            if (string.IsNullOrEmpty(email))
                throw new WebHttpPostAttribute.HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            praises = await praiseLogic.GetPraiseDetails(email, isSender, pageNumber, numberPerPage);

            return praises;
        }

        [MvcHttpPost(Name = "PostPraise")]
        public async Task<PraiseDetails> Post(PraiseDetails praiseSent)
        {
            try
            {
                var email = _contextAccessor.HttpContext.Request.Headers["username"];

                if (string.IsNullOrEmpty(email))
                    throw new WebHttpPostAttribute.HttpResponseException(System.Net.HttpStatusCode.BadRequest);

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
