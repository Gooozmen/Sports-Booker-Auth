using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace CourtBooker.Auth.Infrastructure.Clients;

internal class AzureKeyVaultClient(IConfiguration configuration)
{
    private readonly SecretClient _secretClient = new
    (
        new Uri(configuration["KeyVaultUri"]), 
        new DefaultAzureCredential()
    );

    internal async Task<string> GetSecretAsync(string secretName)
    {
        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately (e.g., logging, retries)
            Console.Error.WriteLine($"Error retrieving secret: {ex.Message}");
            return null;
        }
    }
}