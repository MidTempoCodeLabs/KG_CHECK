using System.ComponentModel;

namespace Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum? enumeration)
    {
        if (enumeration == null)
            return string.Empty;

        var value = enumeration.ToString();
        var type = enumeration.GetType();

        return type.GetField(value)?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[]
        {
            Length: > 0
        } descAttribute
            ? descAttribute[0].Description
            : value;
    }
}