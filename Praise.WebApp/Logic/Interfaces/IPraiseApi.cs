namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseApi
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsync<T>(string url, object? payload = null);
    }
}