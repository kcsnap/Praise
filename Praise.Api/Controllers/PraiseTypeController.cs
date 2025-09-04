using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic;
using Praise.Models;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PraiseTypeController(IHttpContextAccessor _contextAccessor)
    {
        [HttpGet(Name = "GetPraiseTypes")]
        public async Task<IList<PraiseTypeMetadata>> Get()
        {
            var praiseTypesList = new List<PraiseTypeMetadata>();

            foreach (PraiseTypeEnum praiseTypeEnum in Enum.GetValues(typeof(PraiseTypeEnum)))
            {
                praiseTypesList.Add(new PraiseTypeMetadata
                {
                    Type = praiseTypeEnum,
                    Description = praiseTypeEnum.GetDescription(),
                    ImageUrl = "TBD"
                });
            }

            return praiseTypesList;
        }
    }
}
