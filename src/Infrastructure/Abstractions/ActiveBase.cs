using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Base;

public abstract class ActiveBase<T> where T : class
{
    protected void ConfigureActiveProperty(EntityTypeBuilder<T> builder, int order)
    {
        builder.Property("Active")
            .HasDefaultValue(true)
            .HasColumnName("Active")
            .HasColumnOrder(order)
            .IsRequired();
    }
}