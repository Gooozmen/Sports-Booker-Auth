using Shared.Abstractions;

namespace Shared.Wrappers;

public class ApplicationRoleQuery(int propertyType) : PropertyTypeBase(propertyType)
{
    public string Id {get;set;}
    public string Name {get;set;}
    public int GetPropertyType() => propertyType;
}