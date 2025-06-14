namespace CourtBooker.Auth.Presentation;

public static class ConfigValidator
{
    public static void ValidateRequiredConfiguration(IConfiguration config)
    {
        var errors = new List<string>();

        // ConnectionStrings
        if (string.IsNullOrWhiteSpace(config["ConnectionStrings:AuthDb"]))
            errors.Add("ConnectionStrings:AuthDb is missing or empty.");

        // JWT
        if (string.IsNullOrWhiteSpace(config["Jwt:Key"]))
            errors.Add("Jwt:Key is missing or empty.");
        if (string.IsNullOrWhiteSpace(config["Jwt:Issuer"]))
            errors.Add("Jwt:Issuer is missing or empty.");
        if (string.IsNullOrWhiteSpace(config["Jwt:Audience"]))
            errors.Add("Jwt:Audience is missing or empty.");
        
        if (errors.Count > 0)
        {
            throw new InvalidOperationException("Missing required configuration values:\n" + string.Join("\n", errors));
        }
    }
}
