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
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers(o => 
        { 
            o.Filters.Add<ModelStateInterceptor>(); 
            o.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer())); 
        });
        services.AddHttpContextAccessor();
        services.AddScoped<IUserIdentifyService, UserIdentifyService>();
        SetupAuthorization(services);
        return services;
    }
    
   
    public static IConfigurationBuilder AddDefaultConfiguration<T>(this IConfigurationBuilder configurationBuilder) where T : class
    {
        configurationBuilder.AddJsonFile("appsettings.json", true, true);
        configurationBuilder.AddUserSecrets<T>();
        return configurationBuilder;
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

    public static IApplicationBuilder UsePresentationMiddlewares(this IApplicationBuilder app)
        => app.UseMiddleware<UnauthorizeMiddleware>()
              .UseMiddleware<CorrelationIdMiddleware>()
              .UseMiddleware<RequestLoggingMiddleware>();

    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var option = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStringsOption>>();
        services.AddHealthChecks()
            .AddNpgSql(
                option.Value.AuthDb,
                name: "postgresql",
                tags: new[] { "db", "sql" }
            )
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });

        return services;
    }

    public static void MapAppHealthEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = WriteResponse
        });

        app.MapHealthChecks("/api/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("self"),
            ResponseWriter = WriteResponse
        });

        app.MapHealthChecks("/api/health/ready", new HealthCheckOptions
        {
            Predicate = check => !check.Tags.Contains("self"),
            ResponseWriter = WriteResponse
        });
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