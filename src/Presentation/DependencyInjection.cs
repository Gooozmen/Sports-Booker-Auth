using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Presentation.Interceptors;
using Presentation.Middleware;
using Presentation.Services;
using Presentation.Transformations;

namespace Presentation;

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
}