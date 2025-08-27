using System.ComponentModel;

namespace Praise.Models
{
    public class BaseEnumTypeMetadata
    {
        public PraiseTypeEnum Type { get; set; }
        public string Description { get; set; }

    }
    public class PraiseTypeMetadata : BaseEnumTypeMetadata
    {
        public string ImageUrl { get; set; }
    }

    public enum PraiseTypeEnum
    {
        [Description("Thank you!")]
        ThankYou,

        [Description("My Inspiration!")]
        Inspirational,

        [Description("You're a saviour!")]
        Saviour
    }
}
