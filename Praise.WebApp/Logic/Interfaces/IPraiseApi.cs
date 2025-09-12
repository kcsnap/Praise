using Praise.Models.Generic;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseApi
    {
        Task Initialise(IPraiseUserLogic praiseUser);
        Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url);
        Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging paging);
        Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, string querystringParams);
        Task<T> GetAsync<T>(IPraiseUserLogic praiseUser, string url, Paging paging, string querystringParams);
        Task<T> PostAsync<T>(IPraiseUserLogic praiseUser, string url, T payload);
    }
}