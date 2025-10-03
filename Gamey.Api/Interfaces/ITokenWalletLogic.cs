using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface ITokenWalletLogic
    {
        Task<TokenWallet> GetOrCreateWallet(Guid gamerId);
        Task<string> SendToken(string recipientAddress, decimal amount);
        Task<decimal> GetBalance(string walletAddress);

    }
}