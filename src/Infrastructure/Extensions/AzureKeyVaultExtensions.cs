using System.Runtime.InteropServices.ComTypes;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace CourtBooker.Auth.Infrastructure.Extensions;

public static class AzureKeyVaultExtensions
{
    /// <summary>
    /// Adds Azure Key Vault as a configuration source, resolving the VaultUri from existing configuration.
    /// </summary>
    internal static void SetUpAzureKeyVault(this IConfigurationBuilder builder)
    {
        var tempConfig = builder.Build();
        var keyVaultUri = tempConfig["AzureKeyVault:VaultUri"];
        Console.WriteLine($"AzureKeyVault:VaultUri: {keyVaultUri}");

        if (string.IsNullOrWhiteSpace(keyVaultUri))
            throw new InvalidOperationException("AzureKeyVault:VaultUri is missing in configuration.");
        
        builder.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
    }

}