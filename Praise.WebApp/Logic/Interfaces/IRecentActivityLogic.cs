using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IRecentActivityLogic
    {
        Task<RecentActivity> GetRecentActivityForUser(string userEmail);
    }
}