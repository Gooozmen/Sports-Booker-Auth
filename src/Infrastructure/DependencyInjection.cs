using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Database.Seeds;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        //app.UseMiddleware<ResponseInterceptorMiddleware>();
        //app.UseMiddleware<RequestIdMiddleware>();
        return app;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql
            (
                configuration.GetConnectionString("AuthDb"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "Migrations"); // Store migration history in Migrations schema
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.CommandTimeout(15);
                }
            );
            options.EnableDetailedErrors(detailedErrorsEnabled: true);
            options.EnableSensitiveDataLogging();
        });

        // Db Context
        services.AddIdentity<ApplicationUser, ApplicationRole>() // Use ApplicationRole instead of IdentityRole
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>() // Match your custom user and role types
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>() // Match your custom role type
                .AddDefaultTokenProviders();

        services.AddScoped<IDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory<ApplicationDbContext>>();
        services.AddTransient<ApplicationDbContext>(provider => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<DbChangesInterceptor>();
        
        // Seeders
        services.AddScoped<ISeederService, UserSeederService>();

        return services;
    }
}

