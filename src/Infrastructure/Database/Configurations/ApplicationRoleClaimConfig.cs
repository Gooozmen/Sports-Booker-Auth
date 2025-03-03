using Domain.Models;
using Infrastructure.Database.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ApplicationRoleClaimConfig : ActiveBase<ApplicationRoleClaim>,
    IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        // Table name
        builder.ToTable("AspNetRoleClaims", "Identity");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasColumnName("RoleId")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(x => x.ClaimType)
            .HasColumnName("ClaimType")
            .HasColumnOrder(3)
            .IsRequired()
            .HasMaxLength(100); // Assuming a max length for ClaimType

        builder.Property(x => x.ClaimValue)
            .HasColumnName("ClaimValue")
            .HasColumnOrder(4)
            .IsRequired()
            .HasMaxLength(100); // Assuming a max length for ClaimValue

        ConfigureActiveProperty(builder, 5);

        // Foreign key relationship
        builder.HasOne<ApplicationRole>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
    }
}