using CourtBooker.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace CourtBooker.Auth.Infrastructure.Database.EntityConfig;

public class ApplicationUserTokenConfig : IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.ToTable(DatabaseConstants.UserTokens, DatabaseConstants.IdentitySchema);

        // Composite primary key
        builder.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        // Properties
        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(x => x.LoginProvider)
            .HasColumnName("LoginProvider")
            .HasColumnOrder(2)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnOrder(3)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnName("Value")
            .HasColumnOrder(4);

        // Foreign key relationship
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
    }
}