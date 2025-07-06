using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Infrastructure.Database.Seeders;
using CourtBooker.Auth.Application.Environments;
using CourtBooker.Auth.Infrastructure.Environments;
using CourtBooker.Auth.Infrastructure.Factories;
using CourtBooker.Auth.Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CourtBooker.Auth.Infrastructure.Extensions;

namespace CourtBooker.Auth.Infrastructure;

public static class DependencyInjection
{
    public static void AddLoggingInfrastructure(this WebApplicationBuilder builder)
        => builder.SetUpSerilog();
    
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDatabase();
        
        //Services Injection
        services.AddTransient<ITokenFactory, TokenFactory>();
        services.AddSingleton<IEnvironmentValidator, EnvironmentValidator>();
        services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
        services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
        services.AddScoped<ILoginManager, LoginManager>();
        services.AddTransient<ISeeder, ApplicationUserSeeder>();
        services.AddTransient<ISeeder, ApplicationRoleSeeder>();
    }
    public static void AddJwt(this IServiceCollection services)
        => services.SetupJwt();
    
    public static void AddAzureKeyVault(this IConfigurationBuilder builder)
        => builder.SetUpAzureKeyVault();
    
    public static async Task UseEnvironment(this WebApplication app)
        => await app.SetupApplicationEnvironment();
    
    public static void AddAppHealthChecks(this IServiceCollection services)
        => services.SetupAppHealthChecks();
    
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        => services.SetUpOptions(configuration);
}
