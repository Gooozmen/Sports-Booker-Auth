namespace CourtBooker.Auth.Infrastructure.Options;

public sealed class ElasticOption
{
    public required string ApiKey { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}