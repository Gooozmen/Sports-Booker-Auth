namespace CourtBooker.Auth.Infrastructure.Options;

public sealed class ElasticOption
{
    public required string Node { get; set; }
    public required string ApiKey { get; set; }
}