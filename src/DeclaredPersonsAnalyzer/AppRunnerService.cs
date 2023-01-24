using DeclaredPersonsAnalyzer.CmdControllers;
using DeclaredPersonsAnalyzer.CommandLineHelpers.Setups;

namespace DeclaredPersonsAnalyzer;

public class AppRunnerService : IAppRunnerService
{
    private readonly IDeclaredPersonsAnalyzerController _declaredPersonsAnalyzerController;

    public AppRunnerService(IDeclaredPersonsAnalyzerController declaredPersonsAnalyzerController)
    {
        _declaredPersonsAnalyzerController = declaredPersonsAnalyzerController;
    }

    public async Task RunAsync(string[] args)
    {
        var declaredPersonAnalyserOptions =
            DeclaredPersonAnalyserCommandLineParserSetup.GetParsedCmdInputParameters(args);

        if (declaredPersonAnalyserOptions != null)
        {
            var executeRes = await _declaredPersonsAnalyzerController.Execute(declaredPersonAnalyserOptions);

            if (!executeRes.Succeeded)
                throw new Exception(executeRes.Messages.Aggregate((a, b) => a + ", " + b));
        }

        throw new Exception("Options for declared person analyzer are not defined!");
    }
}

public interface IAppRunnerService
{
    Task RunAsync(string[] args);
}