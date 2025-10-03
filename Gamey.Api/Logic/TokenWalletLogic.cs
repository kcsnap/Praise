using Gamey.Api.Configuration;
using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Nethereum.HdWallet;
using Nethereum.Web3;
using System.Numerics;

namespace Gamey.Api.Logic
{
    public class TokenWalletLogic(ILogger<TokenWalletLogic> _logger, IOptions<TokenContractOptions> _tokenContractConfig, ISecureEncryption _secureEncryption, IGameyDbConnection _gameyDbConnection, ITokenWalletDataLayer tokenWalletDataLayer) : ITokenWalletLogic
    {
        public class MaxIndexResult
        {
            public int MaxHdWalletIndex { get; set; }
        }

        private readonly Web3 _web3;

        // Minimal ERC20 ABI for the transfer function
        private const string ERC20Abi = @"[
            {
                'constant': false,
                'inputs': [
                    {'name': '_to', 'type': 'address'},
                    {'name': '_value', 'type': 'uint256'}
                ],
                'name': 'transfer',
                'outputs': [{'name': '', 'type': 'bool'}],
                'type': 'function'
            }
        ]";

        /// <summary>
        /// Find the next available index for a new token wallet
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetNextAvailableIndex()
        {
            
            int? response;

            try
            {
                // requried from ALL games as wallet index is global
                var query = "SELECT VALUE MAX(w.HdWalletIndex) FROM Wallet w";

                // Build query definition
                var queryDef = new QueryDefinition(query: query);

                // Query multiple items from container as raw JSON
                using var filteredFeed = _gameyDbConnection.WalletContainer.GetItemQueryIterator<int>(
                    queryDefinition: queryDef
                );

                // Iterate query result pages
                if (!filteredFeed.HasMoreResults) throw new Exception("error: Hasmoreresults = false in GetNextAvailableIndex");

                FeedResponse<int> maxIndex = await filteredFeed.ReadNextAsync();

                if (maxIndex == null || maxIndex.Count() == 0) throw new Exception("error: Hasmoreresults = null in GetNextAvailableIndex");

                var newIndex = maxIndex.Single() + 1;
                return newIndex;
            }
            catch (Exception ex)
            {
                var error = $"Error in GetNextAvailableIndex, see Inner Exception";
                _logger.LogError(ex, error);
                throw new Exception(error, ex);
            }
        }

        public async Task<TokenWallet> GetOrCreateWallet(Guid gamerId)
        {
            // TODO check wallet index doesn't exist already!
            var wallet = await GetTokenWalletByUserId(gamerId);
            if (wallet != null) return wallet;

            // no wallet for current gamer so lets create one
            int walletIndex = await GetNextAvailableIndex();
            var newWallet = new Wallet(_tokenContractConfig.Value.MagicWords, _tokenContractConfig.Value.Password);
            var account = newWallet.GetAccount(walletIndex);

            var newTokenWallet = new TokenWallet
            {
                GamerId = gamerId,
                HdWalletIndex = walletIndex,
                Address = account.Address,
                PublicKey = account.PublicKey,
                PrivateKeyEncrypted = await _secureEncryption.Encrypt(account.PrivateKey) // TODO - encrypt!
            };

            await SaveTokenWallet(newTokenWallet);

            return newTokenWallet;
        }

        private async Task<TokenWallet> GetTokenWalletByUserId(Guid gamerId)
        {
            return await tokenWalletDataLayer.Read(gamerId);
        }

        private async Task<TokenWallet> SaveTokenWallet(TokenWallet tokenWallet)
        {
            return await tokenWalletDataLayer.Save(tokenWallet);
        }

        public async Task<decimal> GetBalance(string walletAddress)
        {
            var accountAddress = walletAddress;

            //need to specifiy contract address to get balance of specific Tokens
            var contract = _web3.Eth.ERC20.GetContractService(_tokenContractConfig.Value.TokenContractAddress);
            var tokenBalance = await contract.BalanceOfQueryAsync(accountAddress);

            // Convert to token units
            var decimals = await contract.DecimalsQueryAsync();
            var balanceInTokens = Web3.Convert.FromWei(tokenBalance, decimals);

            return balanceInTokens;
        }

        /// <summary>
        /// Sends ERC20 tokens to a recipient address.
        /// </summary>
        /// <param name="recipientAddress">Recipient wallet address</param>
        /// <param name="amount">Amount of tokens (human-readable, not smallest units)</param>
        /// <returns>Transaction hash</returns>
        public async Task<string> SendToken(string recipientAddress, decimal amount)
        {
            // Convert to smallest units
            BigInteger amountInUnits = new BigInteger(amount * (decimal)Math.Pow(10, _tokenContractConfig.Value.TokenDecimals));

            _web3.TransactionManager.UseLegacyAsDefault = true;

            // Get contract and transfer function
            var contract = _web3.Eth.GetContract(ERC20Abi, _tokenContractConfig.Value.TokenContractAddress);
            var transferFunction = contract.GetFunction("transfer");

            // Estimate gas for this token transfer
            BigInteger estimatedGas = await transferFunction.EstimateGasAsync(_tokenContractConfig.Value.TokenOwnerAddress, null, null, recipientAddress, amountInUnits);

            // Add a 10% buffer
            BigInteger gasLimit = estimatedGas + estimatedGas / 10;

            try
            {
                // Send transaction
                string txHash = await transferFunction.SendTransactionAsync(from: _tokenContractConfig.Value.TokenOwnerAddress,
                    gas: new Nethereum.Hex.HexTypes.HexBigInteger(gasLimit),
                    value: null,
                    functionInput: new object[] { recipientAddress, amountInUnits }
                );

                Console.WriteLine($"Transaction sent! Hash: {txHash}");
                return txHash;
            }
            catch (Nethereum.JsonRpc.Client.RpcResponseException ex)
            {
                throw new Exception("What's the error?", ex);
            }
        }
    }
}
