using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseLogic
    {
        Task<PraiseSent> SendPraise(string message);
    }
}