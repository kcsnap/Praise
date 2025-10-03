using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface ITokenWalletDataLayer
    {
        Task<TokenWallet> Read(Guid gamerId);
        Task<TokenWallet> Save(TokenWallet wallet);
    }
}