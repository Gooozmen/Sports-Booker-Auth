
using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.Interfaces;

namespace Shared.Queries;

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