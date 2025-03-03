using Application.Builders;
using Application.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //command handlers
            //Create Handlers
            //query handlers
            
        
        //builders
        services.AddScoped<IApplicationRoleBuilder, ApplicationRoleBuilder>();
        services.AddScoped<IApplicationUserBuilder, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //factories
        services.AddTransient<IPasswordSignInResponseFactory, PasswordSignInResponseFactory>();
        
        
        return services;
    }
    
    public static IServiceCollection AddMediatR<T>(this IServiceCollection services) where T : class
    {
        //Mediator => MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(T).Assembly);
        });

        return services;
    }
}
