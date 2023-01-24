using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAnalyzer.CommandLineHelpers.Constants;
using Fclp;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Setups;

internal static class DeclaredPersonAnalyserCommandLineParserSetup
{
    public static DeclaredPersonAnalyserOptionsRequest? GetParsedCmdInputParameters(string[] args)
    {
        var parser = new FluentCommandLineParser<DeclaredPersonAnalyserOptionsRequest>();

        parser.Setup(arg => arg.Source)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Source).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Source).ToLower()
            )
            .WithDescription("The service address")
            .SetDefault(CommandLineParamsConstants.DeclaredPersonAnalyzer.Source);

        parser.Setup(arg => arg.District)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.District).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.District).ToLower()
                )
            .WithDescription("City id")
            .Required();

        parser.Setup(arg => arg.Year)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Year).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Year).ToLower()
            )
            .WithDescription("The year of record creation");

        parser.Setup(arg => arg.Month)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Month).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Month).ToLower()
            )
            .WithDescription("The month of record creation");

        parser.Setup(arg => arg.Day)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Day).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Day).ToLower()
            )
            .WithDescription("The day of record creation");

        parser.Setup(arg => arg.Limit)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Limit).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Limit).ToLower()
                )
            .WithDescription("The limit of records")
            .SetDefault(CommandLineParamsConstants.DeclaredPersonAnalyzer.Limit);

        parser.Setup(arg => arg.Group)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Group).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Group).ToLower()
            )
            .WithDescription("Allowed values: y, m, d, ym, yd, md");

        parser.Setup(arg => arg.Out)
            .As(
                nameof(DeclaredPersonAnalyserOptionsRequest.Out).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptionsRequest.Out).ToLower()
            )
            .WithDescription("Output file name");
        
        parser.SetupHelp("?", "help")
            .Callback(text => Console.WriteLine(text));

        var result = parser.Parse(args);

        if (result is { HasErrors: true })
        {
            Console.WriteLine("Incorrect command. Please follow these usage instructions:");
            parser.HelpOption.ShowHelp(parser.Options);
            throw new Exception(result.ErrorText);
        }

        return parser.Object;
    }
}
