using Microsoft.AspNetCore.Mvc;
using Praise.Models;
using Praise.WebApp.Logic;
using System.Web.Http;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Praise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PraiseController(IHttpContextAccessor _contextAccessor, IPraiseLogic praiseLogic)
    {
        [HttpPost(Name = "PostPraise")]
        public async Task<PraiseSent> Post(PraiseSent praiseSent)
        {
            var email = _contextAccessor.HttpContext.Request.Headers["username"];

            if (string.IsNullOrEmpty(email))
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            praiseLogic.PublishSendPraiseEvent(praiseSent.To, praiseSent.PraiseType, email, email);
            //praiseLogic.PublishSendPraiseEvent(content);

            return new PraiseSent();
        }
    }
}
