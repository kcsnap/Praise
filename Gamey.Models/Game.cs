namespace Gamey.Api.Models
{
    public class Game
    {
        public Guid? id { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        public string ClientId { get; set; }
        public string EncryptedApiKey { get; set; }
        public IList<BadgeGroupDetails> BadgeGroups{ get; set; }
    }
}
