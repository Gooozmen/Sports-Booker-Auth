using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class OptionExtensions
{
    internal static void SetUpOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind Elastic options
        services.Configure<ElasticOption>(configuration.GetSection("Elastic"));

        // Bind ConnectionStrings
        services.Configure<ConnectionStringsOption>(configuration.GetSection("ConnectionStrings"));

        // Bind Jwt
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));

        // Validate all options
        using var provider = services.BuildServiceProvider();
        provider.ValidateOptions();
    }
    
    private static void ValidateOptions(this IServiceProvider provider)
    {
        var errors = new List<string>();

        try
        {
            var connectionStrings = provider.GetRequiredService<IOptions<ConnectionStringsOption>>().Value;

            if (string.IsNullOrWhiteSpace(connectionStrings.AuthDb))
                errors.Add("ConnectionStringsOption.AuthDb is missing or empty.");
        }
        catch (Exception ex)
        {
            errors.Add($"Failed to load ConnectionStringsOption: {ex.Message}");
        }
        
        try
        {
            var elastic = provider.GetRequiredService<IOptions<ElasticOption>>().Value;

            if (string.IsNullOrWhiteSpace(elastic.Node))
                errors.Add("Elastic.Node is missing or empty.");
            if (string.IsNullOrWhiteSpace(elastic.ApiKey))
                errors.Add("Elastic.ApiKey is missing or empty.");
        }
        catch (Exception ex)
        {
            errors.Add($"Failed to load ConnectionStringsOption: {ex.Message}");
        }

        try
        {
            var jwt = provider.GetRequiredService<IOptions<JwtOption>>().Value;

            if (string.IsNullOrWhiteSpace(jwt.Key))
                errors.Add("JwtOption.Key is missing or empty.");
            if (string.IsNullOrWhiteSpace(jwt.Issuer))
                errors.Add("JwtOption.Issuer is missing or empty.");
            if (string.IsNullOrWhiteSpace(jwt.Audience))
                errors.Add("JwtOption.Audience is missing or empty.");
            if (jwt.ExpiryMinutes <= 0)
                errors.Add("JwtOption.ExpiryMinutes must be greater than 0.");
        }
        catch (Exception ex)
        {
            errors.Add($"Failed to load JwtOption: {ex.Message}");
        }

        if (errors.Any())
            throw new InvalidOperationException("Missing or invalid configuration values:\n" + string.Join("\n", errors));
    }
    
    //--------------------------------------------------------------------------------------------------
    internal static ConnectionStringsOption GetConnectionString(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        return serviceProvider
            .GetRequiredService<IOptions<ConnectionStringsOption>>()
            .Value;
    }
    
    internal static JwtOption GetJwtOption(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        return serviceProvider
            .GetRequiredService<IOptions<JwtOption>>()
            .Value;
    }
    
    internal static ElasticOption GetElasticOption(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        return serviceProvider
            .GetRequiredService<IOptions<ElasticOption>>()
            .Value;
    }
    //------------------------------------------------------------------------------------------------------
}