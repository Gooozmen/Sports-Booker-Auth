using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Infrastructure.Database;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class DatabaseExtensions
{
    internal static void AddDatabase(this IServiceCollection services)
    {
        services.SetupIdentityConfiguration()
                .SetUpDbContext()
                .InjectDbServices();
    }
    
    internal static async Task RunDatabaseInitialization(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
    }

    private static void InjectDbServices(this IServiceCollection services)
    { 
        services.AddScoped<IDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory<ApplicationDbContext>>();
        services.AddTransient<ApplicationDbContext>(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddScoped<ApplicationDbContextInitializer>();
    }
      
    private static IServiceCollection SetUpDbContext(this IServiceCollection services)
        => services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql
                (
                    services.GetConnectionString().AuthDb,
                    npgsqlOptions => SetupNpqslMigrations(npgsqlOptions)
                );
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        );

    private static NpgsqlDbContextOptionsBuilder SetupNpqslMigrations(this NpgsqlDbContextOptionsBuilder builder) 
    => builder.MigrationsHistoryTable("__EFMigrationsHistory", "Migrations")
              .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
              .CommandTimeout(15);

    private static IServiceCollection SetupIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        
        return services;
    }
    
    
}
        
