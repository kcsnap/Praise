using Praise.Models.Generic;

namespace Praise.Api.Logic.Interfaces
{
    public interface IHttpMethodWrapper
    {
        Task Initialise(IPraiseUserLogic _praiseUserLogic, string gameUri, string clientId, string apiKey);
        Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url);
        Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging? paging, string querystringParams);
        Task<T> PostAsync<T>(IPraiseUserLogic praiseUser, string url, T payload);
        Task<S> PostAsync<T, S>(IPraiseUserLogic praiseUser, string url, T payload);
        Task PatchAsync(IPraiseUserLogic praiseUser, string url);

    }
}
