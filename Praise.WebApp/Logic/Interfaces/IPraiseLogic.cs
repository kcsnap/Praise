using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseLogic
    {
        Task<IEnumerable<PraiseDetails>> GetPraisesSent(IPraiseUser praiseUser);
        Task<IEnumerable<PraiseDetails>> GetPraisesReceived(IPraiseUser praiseUser);

        Task<PraiseDetails> SendPraise(IPraiseUser praiseUser, string fromEmail, string toEmail, PraiseTypeMetadata praiseTypeMetadata);
    }
}