using BootstrapBlazor.Components;
using Praise.Models;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Praise.WebApp.Logic
{
    public interface IPraiseTypeLogic
    {
        Task<IList<PraiseTypeMetadata>> GetPraiseTypes();
    }

    public class PraiseTypeLogic : IPraiseTypeLogic
    {
        //public async Task<IDictionary<PraiseTypeEnum, string>> GetPraiseTypes()
        public async Task<IList<PraiseTypeMetadata>> GetPraiseTypes()
        {
            //var praiseTypes = Enum.GetValues(typeof(PraiseTypeEnum)).Cast<PraiseTypeEnum>();
            //return praiseTypes.ToDictionary();

            // local solution
            //Dictionary<PraiseTypeEnum, string> enumDictionary = EnumExtensions.ToDictionaryWithDescription<PraiseTypeEnum>();
            //return enumDictionary;

            // API solution
            var path = "https://localhost:7222/PraiseType";
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    //var product = await response.Content.ReadAsStringAsync();
                    var productTypes = await response.Content.ReadAsAsync<IList<PraiseTypeMetadata>>();
                    return productTypes;
                }

                throw new NotImplementedException();
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine($"Request Exception: {e.Message}");
                throw new NotImplementedException();
            }
            catch (JsonException e)
            {
                //Console.WriteLine($"JSON Deserialization Exception: {e.Message}");
                throw new NotImplementedException();
            }


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

        public static Dictionary<TEnum, string> ToDictionaryWithDescription<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .ToDictionary(
                           keySelector: e => e,
                           elementSelector: e => e.GetDescription()
                       );
        }
    }

    public enum PraiseTypeEnum
    {
        [Description("Thank you!")]
        ThankYou,

        [Description("My Inspiration!")]
        Inspirational,

        [Description("You're a saviour!")]
        Saviour
    }

}
