using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Extensions;

public static class DbContextExtensions
{
    public static DbContext DetectChangesLazyLoading(this DbContext context, bool enabled)
    {
        context.ChangeTracker.AutoDetectChangesEnabled = enabled;
        context.ChangeTracker.LazyLoadingEnabled = enabled;
        context.ChangeTracker.QueryTrackingBehavior =
            enabled ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking;

        return context;
    }

    public static DbSet<T> CommandSet<T>(this DbContext context) where T : class
    {
        return context.DetectChangesLazyLoading(true).Set<T>();
    }

    public static IQueryable<T> QuerySet<T>(this DbContext context) where T : class
    {
        return context.DetectChangesLazyLoading(false).Set<T>().AsNoTracking();
    }

    public static object?[]? PrimaryKeyValues<T>(this DbContext context, object entity)
    {
        return context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.Select(property =>
            entity.GetType().GetProperty(property.Name)?.GetValue(entity, default)).ToArray();
    }

    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }
}