namespace CourtBooker.Auth.Infrastructure.Options;

public sealed record EntityFrameworkOption
{
    public required bool ExecuteRebuild { get; init; }
}