using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;

namespace Shared.Commands.ApplicationUser;

public class PasswordSignInCommand : ICommand
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}