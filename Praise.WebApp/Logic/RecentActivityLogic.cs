using Praise.Models;
using Praise.Models.Generic;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class RecentActivityLogic(IHttpContextAccessor contextAccessor, IPraiseApi praiseApi) : IRecentActivityLogic
    {
        public async Task<RecentActivity> GetRecentActivityForUser()
        {
            // request recent activities for praises sent, received (and eventually badges)
            var sentTask = RetrieveRecentPraisesSent();
            var receivedTask = RetrieveRecentPraisesReceived();

            await Task.WhenAll(
                sentTask,
                receivedTask
            );

            var recentActivity = new RecentActivity()
            {
                PraisesSent = sentTask.Result,
                PraisesReceived = receivedTask.Result
            };

            return recentActivity;
        }

        private async Task<IEnumerable<PraiseDetails>> RetrieveRecentPraisesSent()
        {
            var sent = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(PraiseApi.PraiseUrl, new Paging() { NumberPerPage=3 }, "issender=true");
            return sent;
        }

        private async Task<IEnumerable<PraiseDetails>> RetrieveRecentPraisesReceived()
        {
            var received = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(PraiseApi.PraiseUrl, new Paging() { NumberPerPage = 3 }, "issender=false");
            return received;
        }
    }
}
