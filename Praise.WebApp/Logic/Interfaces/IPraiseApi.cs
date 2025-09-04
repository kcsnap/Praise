namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseApi
    {
        Task Initialise();
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsync<T>(string url, T payload);
    }
}