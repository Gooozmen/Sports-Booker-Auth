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
        services.AddTransient<IApplicationRoleCommandHandler, ApplicationRoleCommandHandler>();
        
        //builders
        services.AddTransient<IApplicationRoleBuilder, ApplicationRoleBuilder>();
        services.AddTransient<IApplicationUserBuilder, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //query handlers
        return services;
    }
}
