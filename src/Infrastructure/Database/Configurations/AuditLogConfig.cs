using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLog","Audit");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("Id")
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(a => a.TableName)
            .HasColumnName("TableName")
            .HasColumnOrder(2)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.RecordId)
            .HasColumnName("RecordId")
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(a => a.ActionType)
            .HasColumnName("ActionType")
            .HasColumnOrder(4)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.UserName)
            .HasColumnName("UserName")
            .HasColumnOrder(5)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.ChangedAt)
            .HasColumnName("ChangedAt")
            .HasColumnOrder(6)
            .IsRequired();

        builder.Property(a => a.OldValues)
            .HasColumnName("OldValues")
            .HasColumnOrder(7)
            .HasColumnType("jsonb");

        builder.Property(a => a.NewValues)
            .HasColumnName("NewValues")
            .HasColumnOrder(8)
            .HasColumnType("jsonb");
    }
}

