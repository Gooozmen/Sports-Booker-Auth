
using System.Text.Json.Serialization;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Interfaces;

namespace CourtBooker.Auth.Shared.Queries;

public sealed record ApplicationRoleQuery : IPropertyType
{
    public ApplicationRoleQuery(string value, int propertyType)
    {
        PropertyType = propertyType;

        switch (propertyType)
        {
            case (int)IdentityPropertyTypes.RoleId:
                Id = value;
                break;
            case (int)IdentityPropertyTypes.RoleName:
                Name = value;
                break;
        }
    }
    public string? Id {get;}
    public string? Name {get;}
    [JsonIgnore]
    public int PropertyType { get; set; }
}