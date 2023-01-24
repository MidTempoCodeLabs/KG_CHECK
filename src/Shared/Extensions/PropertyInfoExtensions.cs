using System.ComponentModel;
using System.Reflection;

namespace Shared.Extensions;

public static class PropertyInfoExtensions
{
    public static string GetDescription(this PropertyInfo propertyInfo) 
    {
        var attrs = (DescriptionAttribute[])propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attrs.Length > 0 ? attrs[0].Description : propertyInfo.Name;
    }
}
