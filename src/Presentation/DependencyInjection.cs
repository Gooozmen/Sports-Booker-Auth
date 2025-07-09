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
           .UseMiddleware<RequestLoggingMiddleware>();
    }

    public static void MapAppHealthEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = WriteResponse
        }).AllowAnonymous();
    }

    private static Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                exception = entry.Value.Exception?.Message,
                duration = entry.Value.Duration.ToString()
            })
        });

        return context.Response.WriteAsync(result);
    }
}