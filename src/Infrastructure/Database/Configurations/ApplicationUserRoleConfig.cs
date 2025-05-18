using Domain.Models;
using Infrastructure.Database.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ApplicationUserRoleConfig : ActiveBase<ApplicationUserRole>, IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        // Table name
        builder.ToTable("AspNetUserRoles", "Identity");

        // Composite primary key
        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .IsRequired()
            .HasColumnOrder(1);

        builder.Property(x => x.RoleId)
            .HasColumnName("RoleId")
            .IsRequired()
            .HasColumnOrder(2);

        ConfigureActiveProperty(builder, 3);

        // Foreign key relationships
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationRole>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}