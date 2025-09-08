using Praise.Models.Generic;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseApi
    {
        Task Initialise();
        Task<T> GetAsync<T>(string url);
        Task<T> GetAsync<T>(string url, Paging paging);
        Task<T> GetAsync<T>(string url, string querystringParams);
        Task<T> GetAsync<T>(string url, Paging paging, string querystringParams);
        Task<T> PostAsync<T>(string url, T payload);
    }
}