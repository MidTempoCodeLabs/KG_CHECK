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