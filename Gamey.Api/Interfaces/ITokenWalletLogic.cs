using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface ITokenWalletLogic
    {
        Task<TokenWallet> GetOrCreateWallet(Guid gamerId);
        Task<string> SendToken(Guid gamerId, decimal amount);
        Task<decimal> GetBalance(Guid gamerId);

    }
}