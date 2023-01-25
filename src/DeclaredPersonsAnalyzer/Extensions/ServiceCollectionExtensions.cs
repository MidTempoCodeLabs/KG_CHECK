using System.Reflection;
using Application.Configurations;
using Application.Interfaces.Services;
using DeclaredPersonsAdapter.Infrastructure.Extensions;
using DeclaredPersonsAnalyzer.CmdControllers;
using DeclaredPersonsAnalyzer.Validations.DeclaredPersonAnalyserCmdArguments;
using Infrastructure.Services;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeclaredPersonsAnalyzer.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void AddDeclaredPersonsAnalyzerAppMappings(this IServiceCollection services)
    {
        services
            .AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    internal static IServiceCollection AddSharedInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IMailService, MailService>();

        return services;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IDeclaredPersonAnalyzerService, DeclaredPersonAnalyzerService>();
    }

    internal static IServiceCollection AddAppCmdControllers(this IServiceCollection services)
    {
        return services
            .AddTransient<IDeclaredPersonsAnalyzerController, DeclaredPersonsAnalyzerController>();
    }
    
    internal static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddTransient<DeclaredPersonAnalyzerCmdArgumentsValidator>();
    }

    internal static IServiceCollection AddAdapterDependencies(this IServiceCollection services)
    {
        services.AddDeclaredPersonsAdapterMappings();
        services.AddDeclaredPersonsAdapterRepositories();
        services.AddDeclaredPersonsAdapterServices();

        return services;
    }

    internal static void ConfigureAppSettingsSections(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var smtpConfig = configuration.GetSection(nameof(SmtpConfiguration));
        services.Configure<SmtpConfiguration>(smtpConfig);
    }
}
