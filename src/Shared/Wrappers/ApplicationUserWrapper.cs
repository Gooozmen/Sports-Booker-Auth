using CourtBooker.Auth.Domain.Models;

namespace CourtBooker.Auth.Shared.Wrappers;

public class ApplicationUserWrapper
{
    public required ApplicationUser ApplicationUser { get; init; }
    public string Password { get; init; } = null!;
}