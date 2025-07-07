namespace CourtBooker.Auth.Infrastructure.Options;

public sealed class JwtOption
{
    public required string Key { get; set; } // Secret key used for signing the token
    public required string Issuer { get; set; } // The issuer of the token (your authentication service)
    public required string Audience { get; set; } // The intended audience for the token (e.g., your API)
    public required int ExpiryMinutes { get; set; } // The token's expiration time in minutes
}