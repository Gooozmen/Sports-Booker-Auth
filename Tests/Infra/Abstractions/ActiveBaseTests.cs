using CourtBooker.Auth.Infrastructure.Database.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourtBooker.Auth.Tests.Infra.Abstractions;

public class ActiveBaseTests
{
    [Fact]
    public void ConfigureActiveProperty_ShouldSetDefaultValueTrueAndRequired()
    {
        // Arrange
        using var context = new TestDbContext();
        var entityType = context.Model.FindEntityType(typeof(DummyEntity));
        var property = entityType!.FindProperty("Active");

        // Assert
        Assert.NotNull(property);
        Assert.False(property.IsNullable);
        Assert.Equal(true, property.GetDefaultValue());
        Assert.Equal("Active", property.GetColumnName());
    }

    private class DummyEntity
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }

    private class DummyActiveConfiguration : ActiveBase<DummyEntity>
    {
        public void ApplyConfiguration(EntityTypeBuilder<DummyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            ConfigureActiveProperty(builder, 0);
        }
    }

    private class TestDbContext : DbContext
    {
        public DbSet<DummyEntity> Entities => Set<DummyEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DummyEntity>();
            new DummyActiveConfiguration().ApplyConfiguration(entity);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ActiveBaseTests");
        }
    }
}