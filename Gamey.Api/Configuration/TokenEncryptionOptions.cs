namespace Gamey.Api.Configuration
{
    public class TokenEncryptionOptions
    {
        public static string ConfigNode => "TokenEncryption";
        public int KeySize { get; set; } = 32;
        public int NonceSize { get; set; } = 12;
        public int TagSize { get; set; } = 16;
        public int SaltSize { get; set; } = 16;
        public string Password { get; set; }
    }
}
