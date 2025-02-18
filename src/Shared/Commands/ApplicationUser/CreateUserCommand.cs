using System.ComponentModel.DataAnnotations;

namespace Shared.Commands.ApplicationUser;

public class CreateUserCommand
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Length(8,16)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).{8,16}$", ErrorMessage = "Password must be 8-16 characters, contain at least one uppercase letter and one special character.")]
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }

}