using CommandLine;
using DeclaredPersonsAnalyzer.CommandLineHelpers.Constants;

namespace DeclaredPersonsAnalyzer.Models;

public class DeclaredPersonAnalyzerCmdArguments
{
    [Option("source", Default = CommandLineParamsConstants.DeclaredPersonAnalyzer.Source, HelpText = "The service address")]
    public string Source { get; set; } = string.Empty;

    [Option("district", Required = true, HelpText = "City id")]
    public int District { get; set; }

    [Option("year", HelpText = "The year of record creation")]
    public int? Year { get; set; }

    [Option( "month", HelpText = "The month of record creation")]
    public int? Month { get; set; }

    [Option( "day", HelpText = "The day of record creation")]
    public int? Day { get; set; }

    [Option("limit", Default = CommandLineParamsConstants.DeclaredPersonAnalyzer.Limit, HelpText = "The limit of records")]
    public int Limit { get; set; }

    [Option("group", HelpText = "Allowed values: y, m, d, ym, yd, md")]
    public string? Group { get; set; }

    [Option("out", HelpText = "Output file name")]
    public string? Out { get; set; }
}
