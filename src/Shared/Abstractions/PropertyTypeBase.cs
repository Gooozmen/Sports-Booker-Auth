namespace Shared.Abstractions;

public abstract class PropertyTypeBase
{
    protected PropertyTypeBase(int propertyType)
    {
        this.propertyType = propertyType;
    }

    protected int  propertyType { get; set; }
}