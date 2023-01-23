using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAnalyzer.CommandLineHelpers.Options;
using Fclp;
using Newtonsoft.Json;

namespace DeclaredPersonsAnalyzer;

public class AppRunnerService : IAppRunnerService
{
    private readonly IDeclaredPersonODataService _declaredPersonODataService;

    public AppRunnerService(IDeclaredPersonODataService declaredPersonODataService)
    {
        _declaredPersonODataService = declaredPersonODataService;
    }

    public async Task RunAsync(DeclaredPersonAnalyserOptions? declaredPersonAnalyserOptions)
    {
        var ss = declaredPersonAnalyserOptions;

        var response = await _declaredPersonODataService.GetAll();
        if (response.Succeeded)
        {
            var json = JsonConvert.SerializeObject(new { data = response.Data }, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            const string path = "./JsonResult/data.json";
            var folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            await File.WriteAllTextAsync(path, json);
        }
        Console.WriteLine("Congrats! It is working");
    }
}

public interface IAppRunnerService
{
    Task RunAsync(DeclaredPersonAnalyserOptions? declaredPersonsAnalyserCmdParseResult);
}