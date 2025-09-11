namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseUser
    {
        Task<string?> GetCurrentUserAsync();
    }
}
