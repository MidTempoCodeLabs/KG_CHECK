using System.ComponentModel;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Options;

public class DeclaredPersonAnalyserOptions
{
    [Description("source")]
    public string? Source { get; set; }

    [Description("district")]
    public int District { get; set; }

    [Description("year")]
    public int Year { get; set; }

    [Description("month")]
    public int Month { get; set; }

    [Description("day")]
    public int Day { get; set; }

    [Description("limit")]
    public int Limit { get; set; }

    [Description("group")]
    public string? Group { get; set; }

    [Description("out")]
    public string? Out { get; set; }
}
