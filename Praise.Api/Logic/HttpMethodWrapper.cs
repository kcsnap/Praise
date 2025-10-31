using Gamey.Models.Converters;
using Praise.Api.Logic.Interfaces;
using Praise.Models.Generic;
using System.Text.Json;

namespace Praise.Api.Logic
{
    public class HttpMethodWrapper(ILogger<HttpMethodWrapper> _logger) : IHttpMethodWrapper
    {
        protected HttpClient client;
        protected string GameUri;

        public async Task Initialise(IPraiseUserLogic _praiseUserLogic, string gameUri, string clientId, string apiKey)
        {
            // ** can't inject IPraiseUserLogic in constructor due to Scoped vs Singleton DI **
            GameUri = gameUri;

            try
            {
                if (client == null)
                {
                    var fromEmail = _praiseUserLogic.PraiseUser.Email;

                    _logger.LogInformation("Instantiating HttpClient to call Praise.Api");
                    client = new HttpClient();
                    client.DefaultRequestHeaders.Add("X-CLIENT-ID", clientId);
                    client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
                    _logger.LogInformation("Instantiated Praise HttpClient client and api key header for " + fromEmail);
                }
            }
            catch (Exception ex)
            {
                var msg = "GameApi.CallApi() => Error initialising client";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }

        #region generic httpmethods - move to separate class and inject

        public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url)
        {
            return await GetAsync<T>(praiseUser, url, null, string.Empty);
        }

        public async Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging? paging, string querystringParams)
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

        public async Task<T> PostAsync<T>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T, T>(praiseUser, HttpMethods.POST, url, payload);
        }
        public async Task<S> PostAsync<T, S>(IPraiseUserLogic praiseUser, string url, T payload)
        {
            return await CallApi<T, S>(praiseUser, HttpMethods.POST, url, payload);
        }
        public async Task PatchAsync(IPraiseUserLogic praiseUser, string url)
        {
            await CallApi(praiseUser, HttpMethods.PATCH, url);
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
            var path = GameUri + url;

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
                                Converters = { new BadgeDetailsConverter() }
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
