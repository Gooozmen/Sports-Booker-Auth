
using Shared.Interfaces;

namespace Shared.Queries;

public sealed record ApplicationRoleQuery : IPropertyType
{
    public string Id {get;set;}
    public string Name {get;set;}
    public int PropertyType { get; set; }
}