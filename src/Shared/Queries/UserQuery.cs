
using System.Text.Json.Serialization;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Interfaces;
using CourtBooker.Auth.Shared.Responses.User;
using MediatR;

namespace CourtBooker.Auth.Shared.Queries;

public sealed record UserQuery : IRequest<UserResponse>, IQuery 
{
    public UserQuery(string value, int propertyType)
    {
        PropertyType = propertyType;

        switch (propertyType)
        {
            case (int)IdentityPropertyTypes.UserId:
                Id = value;
                break;
            case (int)IdentityPropertyTypes.UserName:
                UserName = value;
                break;
            case (int)IdentityPropertyTypes.UserEmail:
                Email = value;
                break;
        }
    }

    public string? Id { get;}
    public string? Email { get;}
    public string? UserName { get;}
    [JsonIgnore]
    public int PropertyType { get; set; }
    
}