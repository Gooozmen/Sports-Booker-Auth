namespace Infrastructure.Options;

public class JwtOption
{
    public string Key { get; set; } // Secret key used for signing the token
    public string Issuer { get; set; } // The issuer of the token (your authentication service)
    public string Audience { get; set; } // The intended audience for the token (e.g., your API)
    public int ExpiryMinutes { get; set; } // The token's expiration time in minutes
}