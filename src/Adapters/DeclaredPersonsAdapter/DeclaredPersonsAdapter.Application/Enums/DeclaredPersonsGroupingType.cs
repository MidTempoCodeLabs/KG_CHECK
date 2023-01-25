using System.ComponentModel;

namespace DeclaredPersonsAdapter.Application.Enums;

public enum DeclaredPersonsGroupingType
{
    [Description("d")]
    ByDay,

    [Description("m")]
    ByMonth,

    [Description("y")]
    ByYear,

    [Description("ym")]
    ByYearAndMonth,

    [Description("yd")]
    ByYearAndDay,

    [Description("md")]
    ByMonthAndDay
}

public static class DeclaredPersonsGroupingTypeMethods
{
    public static DeclaredPersonsGroupingType? GetEnumFromDescription(string description)
    {
        var type = typeof(DeclaredPersonsGroupingType);
        foreach (var field in type.GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute && attribute.Description == description)
                return (DeclaredPersonsGroupingType)(field.GetValue(null) ?? throw new InvalidOperationException());
        }
        
        return null;
    }
}