
namespace Praise.Api.Logic.Interfaces
{
    public interface IPrizeLogic
    {
        Task<int> GetCryptoBalance(IPraiseUserLogic praiseUserLogic);
    }
}