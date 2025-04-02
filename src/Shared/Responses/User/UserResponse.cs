using Shared.Interfaces;

namespace Shared.Responses.User;

public class UserResponse : IResponse 
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool Active { get; set; }
    public bool LockoutEnabled { get; set; }
    public bool IsSuccess { get; set; }
}