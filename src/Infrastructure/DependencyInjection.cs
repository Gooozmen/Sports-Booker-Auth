using System.Text;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Database;
using Infrastructure.Database.Seeders;
using Infrastructure.Environments;
using Infrastructure.Factories;
using Infrastructure.IdentityManagers;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder SetupLoggingInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
        return builder;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        SetupDatabase(services);
        
        services.AddTransient<ITokenFactory, TokenFactory>();

        services.AddSingleton<IEnvironmentValidator, EnvironmentValidator>();
        services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
        services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
        services.AddScoped<ILoginManager, LoginManager>();
        
        
        services.AddTransient<ISeeder, ApplicationUserSeeder>();
        services.AddTransient<ISeeder, ApplicationRoleSeeder>();
        
        return services;
    }
    public static IServiceCollection ConfigureJwt(this IServiceCollection services)
    {
        var option = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOption>>();

        var jwt = option.Value;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
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
        return services;
    }
    private static void SetupDatabase(this IServiceCollection services)
    {
        var option = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStringsOption>>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql
            (
                option.Value.AuthDb,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory",
                        "Migrations"); // Store migration history in Migrations schema
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.CommandTimeout(15);
                }
            );
            options.EnableDetailedErrors(true);
            // options.EnableSensitiveDataLogging();
        });
        
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<IDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory<ApplicationDbContext>>();
        services.AddTransient<ApplicationDbContext>(provider => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddScoped<ApplicationDbContextInitializer>();
    }
    public static async Task UseDevelopEnvironment(this WebApplication app)
    {
        var environmentValidator = app.Services.GetRequiredService<IEnvironmentValidator>();
        if (environmentValidator.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            await app.RunDatabaseInitialization();
        }
    }
    
    private static async Task RunDatabaseInitialization(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
    }
}
