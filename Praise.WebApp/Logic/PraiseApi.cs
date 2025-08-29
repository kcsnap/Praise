using Praise.WebApp.Logic.Interfaces;
using System.Text.Json;

namespace Praise.WebApp.Logic
{
    public class PraiseApi : IPraiseApi
    {
        private string BasePraiseUrl = "https://localhost:7222/";

        // requires auth
        public static string PraiseTypeUrl = "PraiseType";

        // requires auth and specific user
        public static string RecentActivityUrl = "RecentActivity";
        public static string PraiseUrl = "Praise";

        public async Task<T> GetAsync<T>(string url)
        {
            var path = BasePraiseUrl + url;
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {                    
                    var result = await response.Content.ReadAsAsync<T>();
                    return result;
                }

                throw new NotImplementedException("API Request Error: " + await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine($"Request Exception: {e.Message}");
                throw new NotImplementedException(e.Message, e);
            }
            catch (JsonException e)
            {
                //Console.WriteLine($"JSON Deserialization Exception: {e.Message}");
                throw new NotImplementedException(e.Message, e);
            }
        }

        public Task<T> PostAsync<T>(string url, object? payload = null)
        {
            throw new NotImplementedException();
        }
    }
}
