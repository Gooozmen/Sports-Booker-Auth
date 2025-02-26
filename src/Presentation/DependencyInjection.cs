using Infrastructure.Database;
using Presentation.Environments;
using Infrastructure.Options;
using Application;
using Domain.Models;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Presentation;
using Presentation.Environments;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Add configuration options
        services.Configure<ConnectionStringsOption>(configuration.GetSection("ConnectionStrings"));
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));
        
        return services;
    }

    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        //Singletons
        services.AddSingleton<IEnvironmentValidator, EnvironmentValidator>();

        //Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
        });
        
        return services;
    }
    public static IConfigurationBuilder AddDefaultConfiguration<T>(this IConfigurationBuilder configurationBuilder) where T : class
    {
        // Add appsettings.json
        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        // Add user secrets
        configurationBuilder.AddUserSecrets<T>();

        return configurationBuilder;
    }

    public static IApplicationBuilder UsePresentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
    
    /// <summary>
    /// Configures development-only middleware and runs the database initializer.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <param name="environmentValidator">Service to validate the current environment.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    public static async Task UseDevelopEnvironment(this WebApplication app, IEnvironmentValidator environmentValidator)
    {
        // Check if the current environment is development.
        if (environmentValidator.IsDevelopment())
        {
            // Enable the developer exception page to show detailed error information.
            app.UseDeveloperExceptionPage();
            app.ConfigureOpenApi();
            await app.RunDatabaseInitialization();
        }
    }

    private static void ConfigureOpenApi(this WebApplication app)
    {
        // Map the OpenAPI endpoint for Swagger.
        app.MapOpenApi();
        // Configure Swagger UI to expose API documentation.
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
            options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root.
        });
    }

    private static async Task RunDatabaseInitialization(this WebApplication app)
    {
        // Create a new scope to run the database initializer.
        using var scope = app.Services.CreateScope();
        // Resolve the initializer from the DI container.
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        // Initialize the database asynchronously.
        await initializer.InitialiseAsync();
    }
    
}
