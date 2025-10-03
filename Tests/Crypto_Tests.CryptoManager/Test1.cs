using Microsoft.Extensions.Configuration;
using Nethereum.HdWallet;
using Nethereum.Web3;

namespace Crypto_Tests.CryptoManager
{
    [TestClass]
    public sealed class Test1
    {
        string Words = "ripple scissors kick mammal hire column oak again sun offer wealth tomorrow wagon turn fatal";
        string Password1 = "password";
        string _privateKey = "";
        string rpcUrl = "https://data-seed-prebsc-1-s1.binance.org:8545/"; // BNB Testnet RPC
        string mainWallet = "0x7D39170154E8c4997C19285d33624C49cEf2e97b";
        string recipientWallet = "0x7048C1e69B6bAe603A017d227704eD89983Da4e9";

        // **********************************************************
        // this must be updated each time you deploy a new contract!
        string tokenAddress = "***REMOVED-S21***";
        // This hsould be contract address - creator address extracted from this... => "0x7D39170154E8c4997C19285d33624C49cEf2e97b"
        // **********************************************************

        public Test1()
        {
            var builder = new ConfigurationBuilder()
            .AddUserSecrets<Test1>() // Binds UserSecrets to this class
            ;  // Optional: also pull env vars

            IConfiguration Configuration = builder.Build();

            _privateKey = Configuration["TokenContract:PrivateKey"];
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
        public async Task SendTokens()
        {
            var sender = new TokenManager(rpcUrl, _privateKey, tokenAddress, Words, Password1);

            string recipient = recipientWallet;
            decimal amount = 10; // send 10 tokens

            string txHash = await sender.SendToken(recipient, amount);
            Console.WriteLine($"Transaction hash: {txHash}");
        }

        [TestMethod]

        public async Task CreateWallet()
        {
            var wallet1 = new Wallet(Words, Password1);
            
            var account = wallet1.GetAccount(0);
            Console.WriteLine("Account index : " + 0 + " - Address : " + account.Address + " - Private key : " + account.PrivateKey);

            var account2 = wallet1.GetAccount(1);
            Console.WriteLine("Account2 index : " + 1 + " - Address : " + account2.Address + " - Private key : " + account2.PrivateKey);

            var account10 = wallet1.GetAccount(10);
            Console.WriteLine("Account2 index : " + 10 + " - Address : " + account10.Address + " - Private key : " + account10.PrivateKey);

            // send some tokens
            var sender = new TokenManager(rpcUrl, _privateKey, tokenAddress, Words, Password1);
            
            decimal amount1 = 10; // send 10 tokens
            string txHash1 = await sender.SendToken(account.Address, amount1);

            decimal amount2 = 20; // send 20 tokens
            string txHash2 = await sender.SendToken(account2.Address, amount2);

            decimal amount3 = 30; // send 20 tokens
            string txHash3 = await sender.SendToken(account10.Address, amount3);

            // delay for xfer to complete
            Task.Delay(30000).Wait();

            var web3 = new Web3(account, rpcUrl);
            var contract1 = web3.Eth.ERC20.GetContractService(tokenAddress);            
            var tokenBalance1 = await contract1.BalanceOfQueryAsync(account.Address);

            web3 = new Web3(account2, rpcUrl);
            var contract2 = web3.Eth.ERC20.GetContractService(tokenAddress);
            var tokenBalance2 = await contract2.BalanceOfQueryAsync(account2.Address);
            // balance 2 is {5000000000000000} but we've only xferred 20 tokens???

            web3 = new Web3(account10, rpcUrl);
            var contract3 = web3.Eth.ERC20.GetContractService(tokenAddress);
            var tokenBalance3 = await contract3.BalanceOfQueryAsync(account10.Address);
        }

        [TestMethod]
        public async Task GetEthBalance()
        {
            // Retrieves Native Eth balance, NOT COXY TOKENS!
            var web3 = new Web3(rpcUrl);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(mainWallet);
        }

        [TestMethod]
        public async Task GetCoxyBalance()
        {
            var web3 = new Web3(rpcUrl);

            var accountAddress = mainWallet;

            //need to specifiy contract address to get balance of specific Tokens
            var contract = web3.Eth.ERC20.GetContractService(tokenAddress);
            var tokenBalance = await contract.BalanceOfQueryAsync(accountAddress);

            // Convert to token units
            var decimals = await contract.DecimalsQueryAsync();
            var balanceInTokens = Web3.Convert.FromWei(tokenBalance, decimals);

            Console.WriteLine($"COXY Balance: {balanceInTokens}");
        }
    }

    public class TokenWallet
    {
        public int HdWalletIndex { get; set; }
        public string Address { get; set; }
        public string PrivateKey { get; set; }

    }
}
