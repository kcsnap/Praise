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
        // images from here:
        // https://www.flaticon.com/search?style_id=1236&author_id=6976&word=gamification&type=standard
        [Description("Thank you!")]
        [ImageUrl("praise-thankyou.png")]
        ThankYou,

        [Description("My inspiration!")]
        [ImageUrl("praise-inspiration.png")]
        Inspirational,

        [Description("You're a saviour!")]
        [ImageUrl("praise-saviour.png")]
        Saviour,

        [Description("Cheers!")]
        [ImageUrl("praise-cheers.png")]
        Cheers,

        [Description("You're on fire!")]
        [ImageUrl("praise-onfire.png")]
        OnFire,

        [Description("Simply magic!")]
        [ImageUrl("praise-magic.png")]
        Magician
    }
}

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class ImageUrlAttribute : Attribute
{
    public string Url { get; }

    public ImageUrlAttribute(string url)
    {
        Url = url;
    }
}
