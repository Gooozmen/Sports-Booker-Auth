namespace CourtBooker.Auth.Presentation;

public static class ConfigValidator
{
    public static void ValidateRequiredConfiguration(IConfiguration config)
    {
        var errors = new List<string>();

        // ConnectionStrings
        if (string.IsNullOrWhiteSpace(config["ConnectionStrings:AuthDb"]))
            errors.Add("ConnectionStrings:AuthDb is missing or empty.");
        if (string.IsNullOrWhiteSpace(config["ConnectionStrings:Redis"]))
            errors.Add("ConnectionStrings:Redis is missing or empty.");

        // JWT
        if (string.IsNullOrWhiteSpace(config["Jwt:Key"]))
            errors.Add("Jwt:Key is missing or empty.");
        if (string.IsNullOrWhiteSpace(config["Jwt:Issuer"]))
            errors.Add("Jwt:Issuer is missing or empty.");
        if (string.IsNullOrWhiteSpace(config["Jwt:Audience"]))
            errors.Add("Jwt:Audience is missing or empty.");

        // Serilog - Elasticsearch Sink
        var elasticNode = config["Serilog:WriteTo:0:Args:nodes:0"];
        if (string.IsNullOrWhiteSpace(elasticNode))
            errors.Add("Serilog Elasticsearch node URL is missing (Serilog:WriteTo:0:Args:nodes:0).");

        if (string.IsNullOrWhiteSpace(config["Serilog:WriteTo:0:Args:apiKey"]) &&
            (string.IsNullOrWhiteSpace(config["Serilog:WriteTo:0:Args:username"]) ||
             string.IsNullOrWhiteSpace(config["Serilog:WriteTo:0:Args:password"])))
        {
            errors.Add("Either Serilog:WriteTo:0:Args:apiKey or both username/password must be provided.");
        }

        if (errors.Count > 0)
        {
            throw new InvalidOperationException("Missing required configuration values:\n" + string.Join("\n", errors));
        }
    }
}
