namespace CourtBooker.Auth.Infrastructure.Options;

public class ConnectionStringsOption
{
    public required string AuthDb { get; init; }
    public required string Redis { get; init; }
}