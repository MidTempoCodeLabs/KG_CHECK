using Application.Extensions;
using DeclaredPersonsAnalyzer.Extensions;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ILogger = Serilog.ILogger;

namespace DeclaredPersonsAnalyzer;

public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var isDevelopment = environment == "Development";

        Log.Logger = isDevelopment
            ? GetDevEnvLoggerConfiguration()
            : GetProductionEnvLoggerConfiguration();

        try
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationLayer();

                    services.AddDeclaredPersonsAnalyzerAppMappings();
                    services.AddInfrastructureMappings();
                    services.AddRepositories();

                    services.AddApplicationServices();

                    // services.AddSharedInfrastructure();

                    services.AddAdapterDependencies();

                    services.AddValidators();

                    services.AddAppCmdControllers();

                    services.AddTransient<IAppRunnerService, AppRunnerService>();
                })
                .UseSerilog()
                .Build();

            var entryPoint = ActivatorUtilities.CreateInstance<AppRunnerService>(host.Services);
            await entryPoint.RunAsync(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static ILogger GetDevEnvLoggerConfiguration()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: SystemConsoleTheme.Literate)
            .CreateLogger();
    }

    private static ILogger GetProductionEnvLoggerConfiguration(string logsFolder = "logs",
        string logFileName = "log-.txt")
    {
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(Path.Combine(logsFolder, logFileName), rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();
    }
}