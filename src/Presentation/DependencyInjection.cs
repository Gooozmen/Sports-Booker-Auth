using System.Text.Json;
using CourtBooker.Auth.Infrastructure.Options;
using CourtBooker.Auth.Presentation.Interceptors;
using CourtBooker.Auth.Presentation.Middleware;
using CourtBooker.Auth.Presentation.Services;
using CourtBooker.Auth.Presentation.Transformations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Presentation;

public static class DependencyInjection
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers(o => 
        { 
            o.Filters.Add<ModelStateInterceptor>(); 
            o.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer())); 
        });
        services.AddHttpContextAccessor();
        services.AddScoped<IUserIdentifyService, UserIdentifyService>();
        SetupAuthorization(services);
    }
    
    public static IConfigurationBuilder AddDefaultConfiguration(this WebApplicationBuilder hostBuilder)
    {
        var envName = hostBuilder.Environment.EnvironmentName;  // e.g. "Development"
        Console.WriteLine($"ENVIRONMENT {envName}");
        return hostBuilder.Configuration
            .SetBasePath(hostBuilder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>(optional: envName == "Development");
    }

    
   
    public static void AddDefaultConfiguration<T>(this IConfigurationBuilder configurationBuilder) where T : class
    {
        configurationBuilder.AddJsonFile("appsettings.json", false, true);
        configurationBuilder.AddUserSecrets<T>();
    }
    private static void SetupAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }

    public static void UsePresentationMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<UnauthorizeMiddleware>()
           .UseMiddleware<CorrelationIdMiddleware>()
           .UseMiddleware<RequestLoggingMiddleware>()
           .UseMiddleware<HealthCheckTaggingMiddleware>();
    }

    public static void MapAppHealthEndpoints(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger("HealthCheck");
        
        app.MapHealthChecks("/api/health", new HealthCheckOptions
            {
                ResponseWriter = (context, report) => WriteResponse(context, report, logger)
            })
            .AllowAnonymous();
    }
    
    private static Task WriteResponse(HttpContext context, HealthReport report, ILogger logger)
    {
        context.Response.ContentType = "application/json";

        var responseObject = new
        {
            RequestType = "HealthCheck",
            HealthStatus = report.Status.ToString(),
            Checks = report.Entries.Select(entry => new
            {
                Name = entry.Key,
                Status = entry.Value.Status.ToString(),
                Exception = entry.Value.Exception?.Message,
                Duration = entry.Value.Duration.ToString()
            })
        };
        
        logger.LogInformation("HealthCheck result {@HealthCheck}", responseObject);
        
        var result = JsonSerializer.Serialize(responseObject);
        return context.Response.WriteAsync(result);
    }


}