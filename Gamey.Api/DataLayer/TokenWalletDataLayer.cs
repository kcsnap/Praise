using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;

namespace Gamey.Api.DataLayer
{    
    public class TokenWalletDataLayer(ILogger<TokenWalletDataLayer> _logger, IGameyDbConnection _gameyDbConnection) : ITokenWalletDataLayer
    {
        public async Task<TokenWallet> Read(Guid gamerId)
        {
            try
            {
                var query = "SELECT * FROM Wallet g WHERE g.GamerId = @gamerId";

                // Build query definition
                var parameterizedQuery = new QueryDefinition(query: query).WithParameter("@gamerId", gamerId);

                // Query multiple items from container
                using FeedIterator<TokenWallet> filteredFeed = _gameyDbConnection.WalletContainer.GetItemQueryIterator<TokenWallet>(
                    queryDefinition: parameterizedQuery
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("TokenWallet details not found for gamerid:" + gamerId);
                
                FeedResponse<TokenWallet> dbWallets = await filteredFeed.ReadNextAsync();

                if (dbWallets == null || dbWallets.Count() == 0) return null; // indicate no wallet found
                if (dbWallets.Count() > 1) throw new Exception("More than one wallet found for gamerid: " + gamerId);

                return dbWallets.Single();
            }
            catch(Exception ex)
            {
                var error = "Error in TokenWalletDataLayer.Read (gamerid = " + gamerId + ")";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<TokenWallet> Save(TokenWallet wallet)
        {
            try
            {
                // if new item assign id
                if (wallet == null) throw new Exception("walletDataLayer() - wallet cannot be null");
                if(string.IsNullOrEmpty(wallet.Address)) throw new Exception("walletDataLayer - address is required");
                if (string.IsNullOrEmpty(wallet.PrivateKeyEncrypted)) throw new Exception("PrivateKeyEncrypted - private key is required");

                if (wallet.id.HasValue) throw new Exception("id must be null, only wallet creation is permitted");

                // create id
                wallet.id = Guid.NewGuid();

                ItemResponse<TokenWallet> response = await _gameyDbConnection.WalletContainer.UpsertItemAsync(
                    item: wallet,
                    partitionKey: new PartitionKey(wallet.id.ToString())
                );

                return response;
            }
            catch (CosmosException ex)
            {
                var error = "Gamey Error in TokenWalletDataLayer.Save";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }
    }
}
