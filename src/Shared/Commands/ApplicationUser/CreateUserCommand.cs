using System.ComponentModel.DataAnnotations;

namespace Shared.Commands.ApplicationUser;

public class CreateUserCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }

}