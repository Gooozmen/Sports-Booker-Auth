using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using Domain.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interceptors;

public class DbChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

        var auditLogs = new List<AuditLog>();
        var user = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                var auditLog = new AuditLog()
                {
                    TableName = entry.Metadata.GetTableName(),
                    RecordId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue as Guid? ?? Guid.Empty,
                    UserName = user,
                    ChangedAt = DateTime.UtcNow,
                    ActionType = entry.State.ToString(),
                    OldValues = entry.State == EntityState.Modified ? SerializeProperties(entry.OriginalValues) : null,
                    NewValues = entry.State != EntityState.Deleted ? SerializeProperties(entry.CurrentValues) : null
                };

                auditLogs.Add(auditLog);
            }
        }

        eventData.Context.Set<AuditLog>().AddRange(auditLogs);
        return base.SavingChanges(eventData, result);
    }

    private string SerializeProperties(PropertyValues? values)
    {
        if (values == null) return "{}";
        var dict = values.Properties.ToDictionary(p => p.Name, p => values[p]);
        return JsonSerializer.Serialize(dict);
    }
}

