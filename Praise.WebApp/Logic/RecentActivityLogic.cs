using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class RecentActivityLogic(IPraiseApi praiseApi) : IRecentActivityLogic
    {
        public async Task<RecentActivity> GetRecentActivityForUser(string userEmail)
        {
            return await praiseApi.GetAsync<RecentActivity>(PraiseApi.RecentActivityUrl);
        }
    }
}
