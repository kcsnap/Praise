using Praise.Models;
using Praise.Models.Generic;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class RecentActivityLogic(IHttpContextAccessor contextAccessor, IPraiseApi praiseApi) : IRecentActivityLogic
    {
        public async Task<RecentActivity> GetRecentActivityForUser(IPraiseUserLogic praiseUser)
        {
            // request recent activities for praises sent, received (and eventually badges)
            var sentTask = RetrieveRecentPraisesSent(praiseUser);
            var receivedTask = RetrieveRecentPraisesReceived(praiseUser);

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

        private async Task<IEnumerable<PraiseDetails>> RetrieveRecentPraisesSent(IPraiseUserLogic praiseUser)
        {
            var sent = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(praiseUser, PraiseApiWrapper.PraiseUrl, new Paging() { NumberPerPage=3 }, "issender=true");
            return sent;
        }

        private async Task<IEnumerable<PraiseDetails>> RetrieveRecentPraisesReceived(IPraiseUserLogic praiseUser)
        {
            var received = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(praiseUser, PraiseApiWrapper.PraiseUrl, new Paging() { NumberPerPage = 3 }, "issender=false");
            return received;
        }
    }
}
