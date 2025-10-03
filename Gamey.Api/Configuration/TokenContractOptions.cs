namespace Gamey.Api.Configuration
{
    public class TokenContractOptions
    {
        public static string ConfigNode => "TokenContract";
        public string PrivateKey { get; set; }
        public string RpcUrl { get; set; }
        public string TokenContractAddress { get; set; }
        public string TokenOwnerAddress { get; set; }
        public string MagicWords { get; set; }
        public string Password { get; set; }
        public int TokenDecimals { get; set; } = 18;
    }
}
