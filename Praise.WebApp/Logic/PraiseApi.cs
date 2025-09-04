using Praise.WebApp.Logic.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Praise.WebApp.Logic
{
    public class PraiseApi : IPraiseApi
    {
        protected HttpClient client;
        protected IHttpContextAccessor _contextAccessor;
        protected string bearerToken;

        public PraiseApi(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task Initialise()
        {
            // Set the request URL
            var url = "https://dev-snapwire01.uk.auth0.com/oauth/token";

            // Prepare the form data
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", "0XRSfxmNOchXgmGztSDvh0kfMXLMV66R"),
                new KeyValuePair<string, string>("client_secret", "***REMOVED-S21***"),
                new KeyValuePair<string, string>("audience", "https://dev-snapwire01.uk.auth0.com/api/v2/")
            });

            // Send the POST request
            var authClient = new HttpClient();
            var response = Task.Run(async () => await authClient.PostAsync(url, formData)).Result;
            var content = Task.Run(response.Content.ReadAsStringAsync).Result;
            bearerToken = JsonDocument.Parse(content).RootElement.GetProperty("access_token").GetString();

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            client.DefaultRequestHeaders.Add("username", _contextAccessor.HttpContext.User.Identity.Name);
        }

        private string BasePraiseUrl = "https://localhost:7222/";

        // requires auth
        public static string PraiseTypeUrl = "PraiseType";

        // requires auth and specific user
        public static string RecentActivityUrl = "RecentActivity";
        public static string PraiseUrl = "Praise";

        public async Task<T> GetAsync<T>(string url)
        {
            var path = BasePraiseUrl + url;
            return await CallApi<T>(HttpMethods.GET, url);
        }

        public async Task<T> PostAsync<T>(string url, T payload)
        {
            var path = BasePraiseUrl + url;
            return await CallApi<T>(HttpMethods.POST, url, payload);
        }

        public enum HttpMethods
        {
            GET,
            POST
        }

        public async Task<T> CallApi<T>(HttpMethods httpMethod, string url)
        {
            return await CallApi(httpMethod, url, default(T?));
        }
        public async Task<T> CallApi<T>(HttpMethods httpMethod, string url, T? payloadObject)
        {
            if (client == null) Initialise();

            HttpResponseMessage response;
            var path = BasePraiseUrl + url;

            try
            {
                if (httpMethod == HttpMethods.GET)
                {
                    response = await client.GetAsync(path);
                }
                else // if(httpMethod == HttpMethods.POST)
                {
                    response = await client.PostAsJsonAsync<T>(path, payloadObject);
                }

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
    }
}
