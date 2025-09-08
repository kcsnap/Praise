namespace Praise.Models
{
    public class PraiseDetails
    {
        public Guid? id { get; set; }
        public PraiseTypeEnum PraiseType { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PraiseDate { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
    }
}
