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

    public string? Id { get; }
    public string? Email { get; }
    public string? UserName { get; }

    [JsonIgnore] public int PropertyType { get; set; }
}