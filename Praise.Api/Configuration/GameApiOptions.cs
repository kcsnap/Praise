namespace Praise.Api.Configuration
{
    public class GameApiOptions
    {
        public static string ConfigNode => "GameApi";
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string GameUri { get; set; }
    }
}
