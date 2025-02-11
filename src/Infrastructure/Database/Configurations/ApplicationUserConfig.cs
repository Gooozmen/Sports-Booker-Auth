using Domain.Models;
using Infrastructure.Database.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ApplicationUserConfig: ActiveBase<ApplicationUser>, IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Table name
        builder.ToTable("AspNetUsers","Identity");

        // Primary key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(x => x.UserName)
            .HasColumnName("UserName")
            .HasColumnOrder(2)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.NormalizedUserName)
            .HasColumnName("NormalizedUserName")
            .HasColumnOrder(3)
            .HasMaxLength(256);

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasColumnOrder(4)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.NormalizedEmail)
            .HasColumnName("NormalizedEmail")
            .HasColumnOrder(5)
            .HasMaxLength(256);

        builder.Property(x => x.EmailConfirmed)
            .HasColumnName("EmailConfirmed")
            .HasColumnOrder(6)
            .HasDefaultValue(false);

        builder.Property(x => x.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasColumnOrder(7)
            .IsRequired();

        builder.Property(x => x.SecurityStamp)
            .HasColumnName("SecurityStamp")
            .HasColumnOrder(8);

        builder.Property(x => x.ConcurrencyStamp)
            .HasColumnName("ConcurrencyStamp")
            .HasColumnOrder(9);

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasColumnOrder(10);

        builder.Property(x => x.PhoneNumberConfirmed)
            .HasColumnName("PhoneNumberConfirmed")
            .HasColumnOrder(11)
            .HasDefaultValue(false);

        builder.Property(x => x.TwoFactorEnabled)
            .HasColumnName("TwoFactorEnabled")
            .HasColumnOrder(12)
            .HasDefaultValue(false);

        builder.Property(x => x.LockoutEnd)
            .HasColumnName("LockoutEnd")
            .HasColumnOrder(13);

        builder.Property(x => x.LockoutEnabled)
            .HasColumnName("LockoutEnabled")
            .HasColumnOrder(14)
            .HasDefaultValue(false);

        builder.Property(x => x.AccessFailedCount)
            .HasColumnName("AccessFailedCount")
            .HasColumnOrder(15);
        
        ConfigureActiveProperty(builder,16);
    }
}