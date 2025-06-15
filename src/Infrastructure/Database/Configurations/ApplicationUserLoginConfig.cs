using CourtBooker.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace CourtBooker.Auth.Infrastructure.Database.Configurations;

public class ApplicationUserLoginConfig : IEntityTypeConfiguration<ApplicationUserLogin>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
    {
        // Table name
        builder.ToTable(DatabaseConstants.UserLogins, DatabaseConstants.IdentitySchema);

        // Primary key
        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });

        // Properties
        builder.Property(x => x.LoginProvider)
            .HasColumnName("LoginProvider")
            .HasColumnOrder(1)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.ProviderKey)
            .HasColumnName("ProviderKey")
            .HasColumnOrder(2)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.ProviderDisplayName)
            .HasColumnName("ProviderDisplayName")
            .HasColumnOrder(3)
            .HasMaxLength(100);

        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnOrder(4)
            .IsRequired();

        // Foreign key relationship
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
    }
}