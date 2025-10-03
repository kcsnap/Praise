namespace Gamey.Api.Models
{
    public class ActionHistory
    {
        public Guid? id { get; set; }
        public Guid GameId { get; set; }
        public Guid GamerId { get; set; }
        public DateTime ActionDate { get; set; }
        public ActionEnum Action { get; set; }
    }
}
