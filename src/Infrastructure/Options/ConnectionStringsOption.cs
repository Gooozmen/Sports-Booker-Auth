using System.Diagnostics.CodeAnalysis;

namespace CourtBooker.Auth.Infrastructure.Options;

public sealed class ConnectionStringsOption
{

    public required string AuthDb { get; set; }
    public required string Redis { get; set; }
}