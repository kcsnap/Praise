namespace Gamey.Models
{
    public class PrizeGroups
    {
        public Guid? id { get; set; }
        public string PrizeTitle { get; set; }
        public string PrizeDescription { get; set; }
        public PrizeType PrizeType { get; set; }
        public int PrizeAmount { get; set; }
        public Guid GameEventCodeId { get; set; }
        public Guid GameId { get; set; }
    }

    public enum PrizeType
    {
        Crypto
    }
}
