using Microsoft.AspNetCore.Mvc;
using Praise.Models;
using System.ComponentModel;
using System.Reflection;

namespace Praise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PraiseTypeController
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

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return attribute == null ? value.ToString() : attribute.Description;
            }
            return value.ToString();
        }
    }
}
