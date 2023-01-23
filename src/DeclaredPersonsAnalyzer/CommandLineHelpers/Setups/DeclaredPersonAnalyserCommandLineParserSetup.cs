using DeclaredPersonsAnalyzer.CommandLineHelpers.Constants;
using DeclaredPersonsAnalyzer.CommandLineHelpers.Options;
using Fclp;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Setups;

internal class DeclaredPersonAnalyserCommandLineParserSetup
{
    public static DeclaredPersonAnalyserOptions? GetParsedCmdInputParameters(string[] args)
    {
        var parser = new FluentCommandLineParser<DeclaredPersonAnalyserOptions>();

        parser.Setup(arg => arg.Source)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Source).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Source).ToLower()
            )
            .WithDescription("The service address")
            .SetDefault(CommandLineParamsConstants.DeclaredPersonAnalyzer.Source);

        parser.Setup(arg => arg.District)
            .As(
                nameof(DeclaredPersonAnalyserOptions.District).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.District).ToLower()
                )
            .WithDescription("City id")
            .Required();

        parser.Setup(arg => arg.Year)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Year).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Year).ToLower()
            )
            .WithDescription("The year of record creation");

        parser.Setup(arg => arg.Month)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Month).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Month).ToLower()
            )
            .WithDescription("The month of record creation");

        parser.Setup(arg => arg.Day)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Day).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Day).ToLower()
            )
            .WithDescription("The day of record creation");

        parser.Setup(arg => arg.Limit)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Limit).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Limit).ToLower()
                )
            .WithDescription("The limit of records")
            .SetDefault(CommandLineParamsConstants.DeclaredPersonAnalyzer.Limit);

        parser.Setup(arg => arg.Group)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Group).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Group).ToLower()
            )
            .WithDescription("Allowed values: y, m, d, ym, yd, md");

        parser.Setup(arg => arg.Out)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Out).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Out).ToLower()
            )
            .WithDescription("Output file name");

        var result = parser.Parse(args);

        if (result is { HasErrors: true })
        {
            throw new Exception(result.ErrorText);
        }

        return parser.Object;
    }
}
