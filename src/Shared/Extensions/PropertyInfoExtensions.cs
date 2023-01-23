using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions;

public static class PropertyInfoExtensions
{
    public static string GetDescription(this PropertyInfo propertyInfo) 
    {
        var attrs = (DescriptionAttribute[])propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attrs.Length > 0 ? attrs[0].Description : propertyInfo.Name;
    }
}
