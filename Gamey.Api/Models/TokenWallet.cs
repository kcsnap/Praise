namespace Gamey.Api.Models
{
    public class TokenWallet
    {
        public Guid? id { get; set; }
        public Guid GamerId { get; set; }
        public int HdWalletIndex { get; set; }
        public string Address { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKeyEncrypted { get; set; }
    }
}
