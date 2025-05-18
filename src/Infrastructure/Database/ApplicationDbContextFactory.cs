using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CourtBooker.Auth.Infrastructure.Database;

public class ApplicationDbContextFactory<TContext>(IServiceProvider provider) : IDbContextFactory<TContext>
    where TContext : DbContext
{
    public TContext CreateDbContext()
    {
        if (provider == null)
            throw new InvalidOperationException("You must configure an instance of IServiceProvider");

        return ActivatorUtilities.CreateInstance<TContext>(provider);
    }
}