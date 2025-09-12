using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseLogic
    {
        Task<IEnumerable<PraiseDetails>> GetPraisesSent(IPraiseUserLogic praiseUser);
        Task<IEnumerable<PraiseDetails>> GetPraisesReceived(IPraiseUserLogic praiseUser);

        Task<PraiseDetails> SendPraise(IPraiseUserLogic praiseUser, string toEmail, PraiseTypeMetadata praiseTypeMetadata);
    }
}