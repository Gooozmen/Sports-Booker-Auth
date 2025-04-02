
using System.Text.Json.Serialization;
using MediatR;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Responses.User;

namespace Shared.Queries;

public sealed record UserQuery : IRequest<UserResponse>, IQuery 
{
    public UserQuery(string value, int propertyType)
    {
        PropertyType = propertyType;
        
        if (propertyType.Equals((int)IdentityPropertyTypes.UserId)) 
            Id = value;
        else if(propertyType.Equals((int)IdentityPropertyTypes.UserName)) 
            UserName = value;
        else
            Email = value;
    }

    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? LoginProvider { get; set; }
    public string? ProviderKey { get; set; }
    [JsonIgnore]
    public int PropertyType { get; set; }
    
}