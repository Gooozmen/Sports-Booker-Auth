namespace CourtBooker.Auth.Infrastructure.Options;

public sealed class AzureKeyVaultOption
{
    public required string VaultUri { get; init; }
}