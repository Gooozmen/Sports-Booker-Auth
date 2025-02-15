using System.ComponentModel.DataAnnotations;

namespace Shared.Commands.ApplicationUser;

public class CreateUserCommand
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Length(8,16)]
    public string Password { get; set; }
    public string PhoneNumber { get; set; }

}