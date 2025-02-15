using Application.Builders;
using Application.CommandHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //command handlers
        services.AddTransient<IApplicationUserCommandHandler, ApplicationUserCommandHandler>();
        
        //builders
        services.AddTransient<IApplicationUserBuilder, ApplicationUserBuilder>();
        services.AddTransient<HttpResponseBuilder, HttpResponseBuilder>();
        
        //query handlers
        return services;
    }
}
