using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Microsoft.Extensions.Configuration;

namespace Crypto_Tests.CryptoManager
{
    [TestClass]
    public sealed class Test1
    {
        string _privateKey = "";

        public Test1()
        {
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<Test1>() // Binds UserSecrets to this class
            ;  // Optional: also pull env vars

            IConfiguration Configuration = builder.Build();

            _privateKey = Configuration["PrivateKey"];
        }

        [TestInitialize]
        public void TestInit()
        {
            // This method is called before each test method.
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // This method is called after each test method.
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            string rpcUrl = "https://data-seed-prebsc-1-s1.binance.org:8545/"; // BNB Testnet RPC
            string privateKey = _privateKey;

            // **********************************************************
            // this must be updated each time you deploy a new contract!
            string tokenAddress = "***REMOVED-S21***"; // This hsould be contract address - creator address extracted from this... => "0x7D39170154E8c4997C19285d33624C49cEf2e97b"
            // **********************************************************

            var sender = new TokenSender(rpcUrl, privateKey, tokenAddress);

            string recipient = "0x7048C1e69B6bAe603A017d227704eD89983Da4e9";
            decimal amount = 10; // send 10 tokens

            string txHash = await sender.SendToken(recipient, amount);
            Console.WriteLine($"Transaction hash: {txHash}");
        }
    }

    public class TokenSender
    {
        private readonly Web3 _web3;
        private readonly string _tokenContractAddress;
        private readonly int _tokenDecimals;
        private readonly string _ownerAddress;

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

        public TokenSender(string rpcUrl, string privateKey, string tokenContractAddress, int tokenDecimals = 18)
        {
            var account = new Account(privateKey);
            _web3 = new Web3(account, rpcUrl);
            _tokenContractAddress = tokenContractAddress;
            _tokenDecimals = tokenDecimals;
            _ownerAddress = account.Address;
        }

        /// <summary>
        /// Sends ERC20 tokens to a recipient address.
        /// </summary>
        /// <param name="recipient">Recipient wallet address</param>
        /// <param name="amount">Amount of tokens (human-readable, not smallest units)</param>
        /// <returns>Transaction hash</returns>
        public async Task<string> SendToken(string recipient, decimal amount)
        {
            // Convert to smallest units
            BigInteger amountInUnits = new BigInteger(amount * (decimal)Math.Pow(10, _tokenDecimals));

            _web3.TransactionManager.UseLegacyAsDefault = true;

            // Get contract and transfer function
            var contract = _web3.Eth.GetContract(ERC20Abi, _tokenContractAddress);
            var transferFunction = contract.GetFunction("transfer");

            // Estimate gas for this token transfer
            BigInteger estimatedGas = await transferFunction.EstimateGasAsync(_ownerAddress, null, null, recipient, amountInUnits);

            // Add a 10% buffer
            BigInteger gasLimit = estimatedGas + estimatedGas / 10;

            try
            {
                // Send transaction
                string txHash = await transferFunction.SendTransactionAsync(from: _ownerAddress,
                    gas: new Nethereum.Hex.HexTypes.HexBigInteger(gasLimit),
                    value: null,
                    functionInput: new object[] { recipient, amountInUnits }  // ⚠ Pass function parameters as an array
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
