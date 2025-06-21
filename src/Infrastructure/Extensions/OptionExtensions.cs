using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class OptionExtensions
{
    internal static IServiceCollection SetUpOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Add configuration options
        services.Configure<ConnectionStringsOption>(configuration.GetSection("ConnectionStrings"));
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));
        services.Configure<EntityFrameworkOption>(configuration.GetSection("EntityFramework"));
        services.Configure<AzureKeyVaultOption>(configuration.GetSection("AzureKeyVault"));
        return services;
    }
    
    internal static IOptions<ConnectionStringsOption> GetConnectionString(this IServiceCollection services) 
        => services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStringsOption>>();
    
    internal static IOptions<JwtOption> GetJwtOption(this IServiceCollection services)
        => services.BuildServiceProvider().GetRequiredService<IOptions<JwtOption>>();
}