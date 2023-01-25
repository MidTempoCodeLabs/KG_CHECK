using DeclaredPersonsAdapter.Application.Enums;
using System.ComponentModel;

namespace DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;

public class DeclaredPersonAnalyserOptionsRequest
{
    [Description("source")]
    public string Source { get; set; } = string.Empty;

    [Description("district")]
    public int District { get; set; }

    [Description("year")]
    public int? Year { get; set; }

    [Description("month")]
    public int? Month { get; set; }

    [Description("day")]
    public int? Day { get; set; }

    [Description("limit")]
    public int Limit { get; set; }

    [Description("out")]
    public string? Out { get; set; }
    
    public DeclaredPersonsGroupingType? DeclaredPersonsGroupingType { get; set; }

    public bool IsGroupingByYearIncluded()
    {
        return DeclaredPersonsGroupingType is Enums.DeclaredPersonsGroupingType.ByYear
            or Enums.DeclaredPersonsGroupingType.ByYearAndDay
            or Enums.DeclaredPersonsGroupingType.ByYearAndMonth;
    }

    public bool IsGroupingByMonthIncluded()
    {
        return DeclaredPersonsGroupingType is Enums.DeclaredPersonsGroupingType.ByMonth
            or Enums.DeclaredPersonsGroupingType.ByYearAndMonth
            or Enums.DeclaredPersonsGroupingType.ByMonthAndDay;
    }

    public bool IsGroupingByDayIncluded()
    {
        return DeclaredPersonsGroupingType is Enums.DeclaredPersonsGroupingType.ByYearAndDay or Enums.DeclaredPersonsGroupingType.ByDay;
    }
}