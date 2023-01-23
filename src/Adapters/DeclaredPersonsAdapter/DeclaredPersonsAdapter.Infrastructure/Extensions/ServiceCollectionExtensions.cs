using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DeclaredPersonsAdapter.Infrastructure.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Infrastructure.Services;

namespace DeclaredPersonsAdapter.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDeclaredPersonsAdapterMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    public static IServiceCollection AddDeclaredPersonsAdapterRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IDeclaredPersonODataRepository, DeclaredPersonODataRepository>();
    }

    public static IServiceCollection AddDeclaredPersonsAdapterServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IDeclaredPersonODataService, DeclaredPersonODataService>();
    }
}
