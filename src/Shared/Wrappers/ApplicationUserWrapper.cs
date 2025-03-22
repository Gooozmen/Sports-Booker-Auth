using Domain.Models;

namespace Shared.Wrappers;

public class ApplicationUserWrapper
{
    public ApplicationUser ApplicationUser { get; set; }
    public string Password { get; set; }
}