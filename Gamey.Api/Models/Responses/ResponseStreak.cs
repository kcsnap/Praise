namespace Gamey.Api.Models.Responses
{
    public class ResponseStreak
    {
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime LastActiveDate { get; set; }
    }
}
