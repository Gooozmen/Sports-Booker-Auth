namespace Infrastructure.Options;

public class JwtOption
{
    public required string Key { get; init; } // Secret key used for signing the token
    public required string Issuer { get; init; } // The issuer of the token (your authentication service)
    public required string Audience { get; init; } // The intended audience for the token (e.g., your API)
    public required int ExpiryMinutes { get; init; } // The token's expiration time in minutes
}