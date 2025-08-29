using System.ComponentModel;
using System.Reflection;

namespace Praise.Api.Logic
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return attribute == null ? value.ToString() : attribute.Description;
            }
            return value.ToString();
        }
    }
}
