using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseLogic
    {
        Task<PraiseSent> SendPraise(string fromEmail, string toEmail, PraiseTypeMetadata praiseTypeMetadata);
    }
}