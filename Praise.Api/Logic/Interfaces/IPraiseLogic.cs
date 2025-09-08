
using Praise.Api.DataLayer;
using Praise.Models;

namespace Praise.WebApp.Logic
{
    public interface IPraiseLogic
    {
        Task<IEnumerable<PraiseDetails>> GetPraiseDetails(string email, bool isSender, int? pageNumber, int? numberPerPage);
        event EventHandler<SendPraiseEventArgs> SendPraisePublished;

        void PublishSendPraiseEvent(PraiseDetails _praiseDetails);
    }
}