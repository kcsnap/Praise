using Microsoft.Extensions.Options;
using Praise.Models.Configuration;
using Praise.Models.Generic;
using Praise.WebApp.Logic.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Praise.WebApp.Logic
{
    public class PraiseApiWrapper : IPraiseApiWrapper
    {
        protected HttpClient client;
        protected string bearerToken;
        protected readonly AuthorisationOptions _authOptions;
        protected readonly ILogger<PraiseApiWrapper> _logger;
        protected readonly IPraiseUserLogic _praiseUser;

        public PraiseApiWrapper(IOptions<AuthorisationOptions> authOptions, IOptions<PraiseApiOptions> praiseApiOptions, ILogger<PraiseApiWrapper> logger)
        {
            // todo - refine Iconfiguration options to required section
            _authOptions = authOptions.Value;
            _logger = logger;

            BasePraiseUrl = praiseApiOptions.Value.PraiseUri;
        }

        public async Task Initialise(IPraiseUserLogic _praiseUserLogic)
        {            
            var fromEmail = _praiseUserLogic.PraiseUser.Email;

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

            _logger.LogInformation("Instantiating HttpClient to call Praise.Api");
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            client.DefaultRequestHeaders.Add("username", fromEmail);
            _logger.LogInformation("Instantiated HttpClient username header with " + fromEmail);

            // TODO > Check against login count - this is quick and dirty solution...
            // if no praises assume first login and raise event with gamification engine
            _logger.LogInformation("Check first login detected for user " + _praiseUserLogic.PraiseUser.Email);
            await PostAsync(_praiseUserLogic, GameUrl);
        }

        private readonly string BasePraiseUrl;

        // requires auth
        public static string PraiseTypeUrl = "PraiseType";

        // requires auth and specific user
        public static string GameUrl = "Game";
        public static string RecentActivityUrl = "RecentActivity";
        public static string PraiseUrl = "Praise";
        public static string BadgeUrl = "Badge";
        public static string AchievementsPatchUrl = "Achievements/{0}"; // 0 = achievement id

        public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url)
        {
            return await GetAsync<T>(praiseUser, url, null, string.Empty);
        }

        public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, string querystringParams = "")
        {
            return await GetAsync<T>(praiseUser, url, null, querystringParams);
        }

        public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging paging)
        {
            return await GetAsync<T>(praiseUser, url, paging, string.Empty);
        }

        public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging? paging, string querystringParams)
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

            return await CallApi<T>(praiseUser, HttpMethods.GET, url);
        }

        public async Task PostAsync(IPraiseUserLogic praiseUser, string url)
        {
            await CallApi<object>(praiseUser, HttpMethods.POST, url, null);
        }
        public async Task<T> PostAsync<T>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T>(praiseUser, HttpMethods.POST, url, payload);
        }
        public async Task PatchAsync(IPraiseUserLogic praiseUser, string url)
        {
            await CallApi(praiseUser, HttpMethods.PATCH, url);
        }

        public enum HttpMethods
        {
            GET,
            POST,
            PATCH
        }

        protected async Task CallApi(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url)
        {
            await CallApi<object>(praiseUser, httpMethod, url, null);
        }
        public async Task<T> CallApi<T>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url)
        {
            return await CallApi(praiseUser, httpMethod, url, default(T?));
        }
        public async Task<T> CallApi<T>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url, T? payloadObject)
        {
            try 
            {                
                if (client == null) Initialise(praiseUser);
            }
            catch (Exception ex)
            {
                var msg = "PraiseApi.CallApi() => Error initialising client";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }

            HttpResponseMessage response;
            var path = BasePraiseUrl + url;

            try
            {
                if (httpMethod == HttpMethods.GET)
                {
                    response = await client.GetAsync(path);
                }
                else if(httpMethod == HttpMethods.POST)
                {
                    response = await client.PostAsJsonAsync<T>(path, payloadObject);
                }
                else
                {
                    response = await client.PatchAsJsonAsync<T>(path, payloadObject);
                }

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<T>();
                    return result;
                }

                var msg = "API Request Error calling [" + path + "] and payload object [" + payloadObject + "]: " + await response.Content.ReadAsStringAsync();
                _logger.LogError(msg);
                throw new Exception(msg);
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
            catch (Exception ex)
            {
                var msg = "PraiseApi.CallApi() => [See inner exception]";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
    }
}