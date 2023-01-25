using CommandLine;
using DeclaredPersonsAnalyzer.Models;

namespace DeclaredPersonsAnalyzer.CommandLineHelpers.Setups;

internal static class DeclaredPersonAnalyserCommandLineParserSetup
{
    public static DeclaredPersonAnalyzerCmdArguments? GetParsedCmdInputParameters(string[] args)
    {
        try
        {
            // Preprocess of args, because by default it is double dash for long names
            args = args.Select(x => (x[0] == '-' && char.IsLetter(x[1])) ? "-" + x : x).ToArray();

            var result = Parser.Default.ParseArguments<DeclaredPersonAnalyzerCmdArguments>(args);
            return result.Value;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
