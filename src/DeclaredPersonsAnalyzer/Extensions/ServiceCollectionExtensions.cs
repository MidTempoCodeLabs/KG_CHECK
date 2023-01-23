using Application.Configurations;
using Application.Interfaces.Services;
using DeclaredPersonsAdapter.Infrastructure.Extensions;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeclaredPersonsAnalyzer.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSharedInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IMailService, MailService>();

        return services;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
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
