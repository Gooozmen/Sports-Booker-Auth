using Microsoft.AspNetCore.Authorization;
using Presentation.Interceptors;
using Presentation.Services;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers(o => { o.Filters.Add<ModelStateInterceptor>(); });
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
        services.ConfigureApplicationCookie(options => { options.LoginPath = "/Auth/Login"; });
    }
    
}