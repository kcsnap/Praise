using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.Extensions.Options;
using Praise.Api.Configuration;
using Praise.Api.Logic.Interfaces;
using System.Text.Json;

namespace Praise.Api.Logic
{
    // wrapper class to gaming engine
    public class GameLogic : IGameLogic
    {
        protected HttpClient client;
        protected string bearerToken;
        protected readonly ILogger<GameLogic> _logger;
        protected readonly GameApiOptions _gameApiOptions;
        public GameLogic(IOptions<GameApiOptions> gameApiOptions, ILogger<GameLogic> logger)
        {
            // todo - refine Iconfiguration options to required section
            _logger = logger;
            _gameApiOptions = gameApiOptions.Value;
        }

        public async Task RegisterGamer(IPraiseUserLogic _praiseUserLogic)
        {
            // api call to GameEvent
            var response = await PostAsync<NewGameEvent, ResponseGameBadges>(_praiseUserLogic, GameEventUrl, new NewGameEvent
            {
                EventCode = "register-event",
                GamerId = _praiseUserLogic.PraiseUser.Email
            });
        }


        public async Task Initialise(IPraiseUserLogic _praiseUserLogic)
        {
            var fromEmail = _praiseUserLogic.PraiseUser.Email;

            _logger.LogInformation("Instantiating HttpClient to call Praise.Api");
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CLIENT-ID", _gameApiOptions.ClientId);
            client.DefaultRequestHeaders.Add("X-API-KEY", _gameApiOptions.ApiKey);
            _logger.LogInformation("Instantiated Praise HttpClient username header for " + fromEmail);
        }

        // requires auth and specific user
        public static string GameEventUrl = "GameEvent";

        //public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url)
        //{
        //    return await GetAsync<T>(praiseUser, url, null, string.Empty);
        //}

        //public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, string querystringParams = "")
        //{
        //    return await GetAsync<T>(praiseUser, url, null, querystringParams);
        //}

        //public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging paging)
        //{
        //    return await GetAsync<T>(praiseUser, url, paging, string.Empty);
        //}

        //public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging? paging, string querystringParams)
        //{
        //    // handle querystring...
        //    if (!string.IsNullOrEmpty(querystringParams) && paging != null)
        //        // we have some query data to handle...
        //        url += "?" + querystringParams + "&pagenumber=" + paging.PageNumber + "&numberperpage=" + paging.NumberPerPage;
        //    else if (!string.IsNullOrEmpty(querystringParams))
        //        url += "?" + querystringParams;
        //    else if (paging != null)
        //        url += "?pagenumber=" + paging.PageNumber + "&numberperpage=" + paging.NumberPerPage;
        //    // else = add nothing...

        //    return await CallApi<T>(praiseUser, HttpMethods.GET, url);
        //}

        public async Task<T> PostAsync<T>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T, T>(praiseUser, HttpMethods.POST, url, payload);
        }
        public async Task<S> PostAsync<T, S>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T, S>(praiseUser, HttpMethods.POST, url, payload);
        }

        public enum HttpMethods
        {
            GET,
            POST
        }

        //public async Task<T> CallApi<T>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url)
        //{
        //    return await CallApi(praiseUser, httpMethod, url, default(T?));
        //}
        public async Task<T> CallApi<T>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url, T? payloadObject)
        { 
            return await CallApi<T, T>(praiseUser, httpMethod, url, payloadObject);
        }
        public async Task<S> CallApi<T, S>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url, T? payloadObject)
        {
            try
            {
                if (client == null) Initialise(praiseUser);
            }
            catch (Exception ex)
            {
                var msg = "GameApi.CallApi() => Error initialising client";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }

            HttpResponseMessage response;
            var path = _gameApiOptions.GameUri + url;

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
                    var result = await response.Content.ReadAsAsync<S>();
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
                var msg = "GameApi.CallApi() => " + ex.Message;
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
    }
}
