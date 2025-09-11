using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseLogic
    {
        Task<IEnumerable<PraiseDetails>> GetPraisesSent();
        Task<IEnumerable<PraiseDetails>> GetPraisesReceived();

        Task<PraiseDetails> SendPraise(string fromEmail, string toEmail, PraiseTypeMetadata praiseTypeMetadata);
    }
}