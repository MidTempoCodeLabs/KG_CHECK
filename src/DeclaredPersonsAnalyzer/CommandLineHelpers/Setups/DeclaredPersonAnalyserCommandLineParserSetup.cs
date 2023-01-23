using DeclaredPersonsAnalyzer.CommandLineHelpers.Options;
using Fclp;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Setups;

internal class DeclaredPersonAnalyserCommandLineParserSetup
{
    public static DeclaredPersonAnalyserOptions? GetParsedCmdInputParameters(string[] args)
    {
        var parser = new FluentCommandLineParser<DeclaredPersonAnalyserOptions>();
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
            .WithDescription("The year of record creation")
            .Required();

        parser.Setup(arg => arg.Limit)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Limit).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Limit).ToLower()
                )
            .WithDescription("The limit of records")
            .SetDefault(0);

        parser.Setup(arg => arg.Group)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Group).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Group).ToLower()
                )
            .WithDescription("Grouping by year and month(ym), year and day(yd) or month and day(md)")
            .SetDefault("y");

        parser.Setup(arg => arg.Out)
            .As(
                nameof(DeclaredPersonAnalyserOptions.Out).ToLower()[0],
                nameof(DeclaredPersonAnalyserOptions.Out).ToLower()
                )
            .WithDescription("Output file name")
            .SetDefault("res.json");

        var result = parser.Parse(args);

        if (result is { HasErrors: true })
        {
            throw new Exception(result.ErrorText);
        }

        return parser.Object;
    }
}
