using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Commands.ApplicationUser;

public class PasswordSignInCommand
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}