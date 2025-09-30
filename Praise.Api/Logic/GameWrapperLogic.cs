using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.Extensions.Options;
using Praise.Api.Configuration;
using Praise.Api.Logic.Interfaces;
using Praise.Models.Generic;
using System.Text.Json;

namespace Praise.Api.Logic
{
    // wrapper class to gaming engine
    public class GameWrapperLogic : IGameWrapperLogic
    {
        // requires auth and specific user
        public static string _badgesUrl = "badges";
        public static string _gameEventUrl = "gameevent";
        public static string _gamerUrl = "gamers/{0}"; // 0 = gamerEmail
        public static string _achievementsUrl = "gamers/{0}/achievements"; // 0 = gamerId
        public static string _achievementsPatchUrl = "gamers/{0}/achievements/{1}"; // 0 = gamerId, 1 = achievementId

        protected HttpClient client;
        protected string bearerToken;
        protected readonly ILogger<GameWrapperLogic> _logger;
        protected readonly GameApiOptions _gameApiOptions;

        protected Gamer _gamer;

        public GameWrapperLogic(IOptions<GameApiOptions> gameApiOptions, ILogger<GameWrapperLogic> logger)
        {
            // todo - refine Iconfiguration options to required section
            _logger = logger;
            _gameApiOptions = gameApiOptions.Value;
        }

        public async Task Initialise(IPraiseUserLogic _praiseUserLogic)
        {
            // ** can't inject IPraiseUserLogic in constructor due to Scoped vs Singleton DI **

            // if previously initialised...
            if (client != null) return;

            try
            {
                var fromEmail = _praiseUserLogic.PraiseUser.Email;

                _logger.LogInformation("Instantiating HttpClient to call Praise.Api");
                client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-CLIENT-ID", _gameApiOptions.ClientId);
                client.DefaultRequestHeaders.Add("X-API-KEY", _gameApiOptions.ApiKey);
                _logger.LogInformation("Instantiated Praise HttpClient username header for " + fromEmail);

                // get gamer details on load
                await LoadGamer(_praiseUserLogic);
            }
            catch (Exception ex)
            {
                var msg = "GameApi.CallApi() => Error initialising client";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }

        protected async Task LoadGamer(IPraiseUserLogic _praiseUserLogic)
        {
            var path = string.Format(_gamerUrl, _praiseUserLogic.PraiseUser.Email);
            var response = await GetAsync<Gamer>(_praiseUserLogic, path);
            _gamer = response;
        }

        public async Task RegisterGamer(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);

            // api call to GameEvent
            var response = await PostAsync<NewGameEvent, ResponseGameBadges>(_praiseUserLogic, _gameEventUrl, new NewGameEvent
            {
                EventCode = "register-event",
                GamerEmail = _praiseUserLogic.PraiseUser.Email
            });
        }

        public async Task<IEnumerable<Achievement>> GetAchievements(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);
            var path = string.Format(_achievementsUrl, _gamer.id);

            // api call to GameEvent
            var response = await GetAsync<IEnumerable<Achievement>>(_praiseUserLogic, path);
            return response;
        }
        public async Task UpdateAchievements(IPraiseUserLogic _praiseUserLogic, Guid achievementId)
        {
            await Initialise(_praiseUserLogic);
            var path = string.Format(_achievementsPatchUrl, _gamer.id, achievementId);

            // api call to GameEvent
            await PatchAsync(_praiseUserLogic, path);
        }

        public async Task<IEnumerable<BadgeGroupDetails>> GetAllBadges(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);

            var response = await GetAsync<IEnumerable<BadgeGroupDetails>>(_praiseUserLogic, _badgesUrl);
            return response;
        }

        #region generic httpmethods - move to separate class and inject

        protected async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url)
        {
            return await GetAsync<T>(praiseUser, url, null, string.Empty);
        }

        protected async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging? paging, string querystringParams)
        {
            // handle querystring...
            if (!string.IsNullOrEmpty(querystringParams) && paging != null)
                // we have some query data to handle...
                url += "?" + querystringParams + "&pagenumber=" + paging.PageNumber + "&numberperpage=" + paging.NumberPerPage;
            else if (!string.IsNullOrEmpty(querystringParams))
                url += "?" + querystringParams;
            else if (paging != null)
                url += "?pagenumber=" + paging.PageNumber + "&numberperpage=" + paging.NumberPerPage;
            // else = add nothing...

            return await CallApi<T>(praiseUser, HttpMethods.GET, url);
        }

        protected async Task<T> PostAsync<T>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T, T>(praiseUser, HttpMethods.POST, url, payload);
        }
        protected async Task<S> PostAsync<T, S>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T, S>(praiseUser, HttpMethods.POST, url, payload);
        }
        protected async Task PatchAsync(IPraiseUserLogic praiseUser, string url)
        {
            await CallApi(praiseUser, HttpMethods.PATCH, url);
        }

        protected enum HttpMethods
        {
            GET,
            POST,
            PATCH
        }
        protected async Task CallApi(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url)
        {
            await CallApi<object>(praiseUser, httpMethod, url, null);
        }
        protected async Task<T> CallApi<T>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url)
        {
            return await CallApi(praiseUser, httpMethod, url, default(T?));
        }
        protected async Task<T> CallApi<T>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url, T? payloadObject)
        { 
            return await CallApi<T, T>(praiseUser, httpMethod, url, payloadObject);
        }
        protected async Task<S> CallApi<T, S>(IPraiseUserLogic praiseUser, HttpMethods httpMethod, string url, T? payloadObject)
        {            
            HttpResponseMessage response;
            var path = _gameApiOptions.GameUri + url;

            try
            {
                if (httpMethod == HttpMethods.GET)
                {
                    response = await client.GetAsync(path);
                }
                else if (httpMethod == HttpMethods.POST)
                {
                    response = await client.PostAsJsonAsync<T>(path, payloadObject);
                }
                else
                {
                    response = await client.PatchAsJsonAsync<T>(path, payloadObject);
                }

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.Content.ReadAsAsync<S>();
                        return result;
                    }
                    catch (Newtonsoft.Json.JsonSerializationException e)
                    {
                        // this is likely due to abstract class deserialization - try System.Text.Json with converter
                        try
                        {
                            if (response == null) throw new Exception("Response is null");

                            // casting to abstract or interface??? Try string first...
                            var json = await response.Content.ReadAsStringAsync();

                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                Converters = { new BadgeDetailsConverter() } // client-side converter
                            };

                            var badges = JsonSerializer.Deserialize<S>(json, options);
                            return badges;
                        }
                        catch (Exception ex)
                        {
                            var msg2 = "GameApi.CallApi() => " + ex.Message;
                            _logger.LogError(msg2, ex);
                            throw;
                        }
                    }
                }

                var msg = "API Request Error calling [" + path + "] and payload object [" + payloadObject + "]: " + await response.Content.ReadAsStringAsync();
                _logger.LogError(msg);
                throw new Exception(msg);
            }            
            catch (HttpRequestException ex)
            {
                var msg = "GameApi.CallApi() => " + ex.Message;
                _logger.LogError(msg, ex);
                throw new NotImplementedException(ex.Message, ex);
            }
            catch (JsonException ex)
            {
                var msg = "GameApi.CallApi() => " + ex.Message;
                _logger.LogError(msg, ex);
                throw new NotImplementedException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                var msg = "GameApi.CallApi() => " + ex.Message;
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
        #endregion
    }
}
