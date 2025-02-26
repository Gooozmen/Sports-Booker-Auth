using System.Text;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Database;
using Infrastructure.Database.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.IdentityManagers;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
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
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>() // Match your custom user and role types
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>() // Match your custom role type
                .AddDefaultTokenProviders();

        services.AddScoped<IDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory<ApplicationDbContext>>();
        services.AddTransient<ApplicationDbContext>(provider => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddScoped<ApplicationDbContextInitializer>();
        
        //Identity
        services.AddTransient<IApplicationUserManager, ApplicationUserManager>();
        services.AddTransient<IApplicationRoleManager, ApplicationRoleManager>();
        
        //Seeders
        services.AddTransient<ISeeder, ApplicationUserSeeder>();
        services.AddTransient<ISeeder, ApplicationRoleSeeder>();

        return services;
    }

    public static IServiceCollection ConfigureJWT(this IServiceCollection services, IOptions<JwtOption> jwtOption )
    {
        var jwt = jwtOption.Value;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(jwt.Key);
            
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt.Issuer,
                ValidAudience = jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        return services;
    }
}

