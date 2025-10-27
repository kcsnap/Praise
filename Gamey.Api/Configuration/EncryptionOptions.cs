namespace Gamey.Api.Configuration
{
    public class EncryptionOptions
    {
        public static string ConfigNode => "EncryptionOptions";
        public int KeySize { get; set; } = 32;
        public int NonceSize { get; set; } = 12;
        public int TagSize { get; set; } = 16;
        public int SaltSize { get; set; } = 16;
        public string MasterKey { get; set; }
        public string Password { get; set; }
    }
}
