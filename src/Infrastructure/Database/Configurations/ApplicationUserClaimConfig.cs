

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Database.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ApplicationUserClaimConfig: ActiveBase<ApplicationUserClaim>, IEntityTypeConfiguration<ApplicationUserClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
    {
        builder.ToTable("AspNetUserClaims","Identity");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(x => x.ClaimType)
            .HasColumnName("ClaimType")
            .HasColumnOrder(3)
            .HasMaxLength(100); // Assuming a max length for ClaimType

        builder.Property(x => x.ClaimValue)
            .HasColumnName("ClaimValue")
            .HasColumnOrder(4)
            .HasMaxLength(100)
            .IsRequired(); // Assuming a max length for ClaimValue
        
        ConfigureActiveProperty(builder,5);

        // Foreign key relationship
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
    }
}