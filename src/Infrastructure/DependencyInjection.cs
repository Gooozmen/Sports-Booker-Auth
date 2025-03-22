using System.Text;
using Domain.Models;
using Infrastructure.Database;
using Infrastructure.Database.Seeders;
using Infrastructure.Factories;
using Infrastructure.IdentityManagers;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory",
                        "Migrations"); // Store migration history in Migrations schema
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.CommandTimeout(15);
                }
            );
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging();
        });

        // Db Context
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        services
            .AddScoped<IDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory<ApplicationDbContext>>();
        services.AddTransient<ApplicationDbContext>(provider =>
            provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddScoped<ApplicationDbContextInitializer>();

        //Identity
        services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
        services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
        services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
        
        //Unit Of Work for Managers
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Jwt
        services.AddTransient<ITokenFactory, TokenFactory>();

        //Seeders
        services.AddTransient<ISeeder, ApplicationUserSeeder>();
        services.AddTransient<ISeeder, ApplicationRoleSeeder>();


        return services;
    }

    public static IServiceCollection ConfigureJwt(this IServiceCollection services)
    {
        var option = services.BuildServiceProvider()
            .GetRequiredService<IOptions<JwtOption>>();

        var jwt = option.Value;
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

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Add configuration options
        services.Configure<ConnectionStringsOption>(configuration.GetSection("ConnectionStrings"));
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));
        services.Configure<EntityFrameworkOption>(configuration.GetSection("EntityFramework"));
        services.Configure<RedisOption>(configuration.GetSection("Redis"));
        return services;
    }
}