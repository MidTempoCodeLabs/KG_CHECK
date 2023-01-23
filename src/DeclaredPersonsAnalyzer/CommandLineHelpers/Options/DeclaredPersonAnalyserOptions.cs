using System.ComponentModel;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Options;

public class DeclaredPersonAnalyserOptions
{
    [Description("district")]
    public int District { get; set; }

    [Description("year")]
    public int Year { get; set; }

    [Description("limit")]
    public int Limit { get; set; }

    [Description("group")]
    public string Group { get; set; }

    [Description("out")]
    public string Out { get; set; }
}
