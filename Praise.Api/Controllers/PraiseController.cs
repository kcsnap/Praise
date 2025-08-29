using Microsoft.AspNetCore.Mvc;
using Praise.Models;

namespace Praise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PraiseController
    {
        [HttpGet(Name = "PostPraise")]
        public async Task<PraiseSent> Post()
        {
            return new PraiseSent();
        }
    }
}
