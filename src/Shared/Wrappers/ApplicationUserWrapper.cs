using Domain.Models;

namespace Shared.Wrappers;

public class ApplicationUserWrapper
{
    public required ApplicationUser ApplicationUser { get; init; }
    public string Password { get; init; } = null!;
}