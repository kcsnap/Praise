namespace Praise.Models.Configuration
{
    public class AuthorisationOptions
    {
        public const string ConfigNode = "Authorisation";

        public string AuthTokenUrl { get; set; }
        public string GrantType { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
    }
}
