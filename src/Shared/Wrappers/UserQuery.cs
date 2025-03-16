
using Shared.Abstractions;
using Shared.Enums;

namespace Shared.Wrappers;

public class UserQuery(int propertyType) : PropertyTypeBase(propertyType)
{
    public UserQuery(string email, string password) : this((int)IdentityPropertyTypes.UserEmail)
    {
        Email = email;
        Password = password;
    }

    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? LoginProvider { get; set; }
    public string? ProviderKey { get; set; }
    public int GetPropertyType() => propertyType;
}