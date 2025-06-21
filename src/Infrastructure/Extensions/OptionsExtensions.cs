using System.Diagnostics;
using System.Text.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class OptionExtensions
{
    internal static void SetupAzureKeyVaultClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetUpAzureKeyVaultOptions(configuration);
        var keyVaultUri = services.GetAzureKeyVaultOptions().VaultUri;
        var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        services.SetUpOptions(secretClient);
    }
    
    private static void SetUpAzureKeyVaultOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("AzureKeyVault");
        var ef = configuration.GetSection("EntityFramework");

        var options = section.Get<AzureKeyVaultOption>();
        if (options == null || string.IsNullOrWhiteSpace(options.VaultUri))
            throw new InvalidOperationException("AzureKeyVaultOption.VaultUri is missing or empty.");

        services.Configure<AzureKeyVaultOption>(section);
    }

    
    private static void SetUpOptions(this IServiceCollection services, SecretClient client)
    {
        // Add configuration options
        var connectionStringsSecret = client.GetSecret("ConnectionStrings");
        var connectionStringJson = JsonSerializer.Deserialize<ConnectionStringsOption>(connectionStringsSecret.Value.Value);
        var jwtSecrets = client.GetSecret("Jwt");
        var jwtJson = JsonSerializer.Deserialize<JwtOption>(jwtSecrets.Value.Value);
        
        services
        .Configure<ConnectionStringsOption>(_ =>
        {
            _.AuthDb = connectionStringJson.AuthDb;
            _.Elastic = connectionStringJson.Elastic;
            _.Redis = connectionStringJson.Redis;
        }).Configure<JwtOption>(_ =>
        {
            _.JwtKey = jwtJson.JwtKey;
            _.Audience = jwtJson.Audience;
            _.Issuer = jwtJson.Issuer;
            _.ExpiryMinutes = jwtJson.ExpiryMinutes;
        });

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
            if (string.IsNullOrWhiteSpace(connectionStrings.Elastic))
                errors.Add("ConnectionStringsOption.Elastic is missing or empty.");
        }
        catch (Exception ex)
        {
            errors.Add($"Failed to load ConnectionStringsOption: {ex.Message}");
        }

        try
        {
            var jwt = provider.GetRequiredService<IOptions<JwtOption>>().Value;

            if (string.IsNullOrWhiteSpace(jwt.JwtKey))
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
    private static AzureKeyVaultOption GetAzureKeyVaultOptions(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        return serviceProvider
            .GetRequiredService<IOptions<AzureKeyVaultOption>>()
            .Value;
    }
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
}