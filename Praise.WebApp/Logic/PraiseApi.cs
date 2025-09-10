using Microsoft.Extensions.Options;
using Praise.Models.Configuration;
using Praise.Models.Generic;
using Praise.WebApp.Logic.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Praise.WebApp.Logic
{
    public class PraiseApi : IPraiseApi
    {
        protected HttpClient client;
        protected string bearerToken;

        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly AuthorisationOptions _authOptions;
        
        public PraiseApi(IOptions<AuthorisationOptions> authOptions, IOptions<PraiseApiOptions> praiseApiOptions, IHttpContextAccessor contextAccessor)
        {
            // todo - refine Iconfiguration options to required section
            _authOptions = authOptions.Value;
            _contextAccessor = contextAccessor;

            BasePraiseUrl = praiseApiOptions.Value.PraiseUri;
        }

        public async Task Initialise()
        {
            // Set the request URL
            var url = _authOptions.AuthTokenUrl;
            
            // Prepare the form data
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", _authOptions.GrantType),
                new KeyValuePair<string, string>("client_id", _authOptions.ClientId),
                new KeyValuePair<string, string>("client_secret", _authOptions.ClientSecret),
                new KeyValuePair<string, string>("audience", _authOptions.Audience)
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

        private readonly string BasePraiseUrl;

        // requires auth
        public static string PraiseTypeUrl = "PraiseType";

        // requires auth and specific user
        public static string RecentActivityUrl = "RecentActivity";
        public static string PraiseUrl = "Praise";

        public async Task<T> GetAsync<T>(string url)
        {
            return await GetAsync<T>(url, null, string.Empty);
        }

        public async Task<T> GetAsync<T>(string url, string querystringParams = "")
        {
            return await GetAsync<T>(url, null, querystringParams);
        }

        public async Task<T> GetAsync<T>(string url, Paging paging)
        {
            return await GetAsync<T>(url, paging, string.Empty);
        }

        public async Task<T> GetAsync<T>(string url, Paging? paging, string querystringParams)
        {
            // handle querystring...
            if (!string.IsNullOrEmpty(querystringParams) && paging != null)
                // we have some query data to handle...
                url += "?" + querystringParams + "&pagenumber=" + paging.PageNumber + "&numberperpage=" + paging.NumberPerPage;
            else if (!string.IsNullOrEmpty(querystringParams))
                url += "?" + querystringParams;
            else if(paging != null)
                url += "?pagenumber=" + paging.PageNumber + "&numberperpage=" + paging.NumberPerPage;
            // else = add nothing...

            return await CallApi<T>(HttpMethods.GET, url);
        }

        public async Task<T> PostAsync<T>(string url, T payload)
        {
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
            catch (Exception e)
            {
                //Console.WriteLine($"JSON Deserialization Exception: {e.Message}");
                throw new NotImplementedException(e.Message, e);
            }
        }
    }
}
