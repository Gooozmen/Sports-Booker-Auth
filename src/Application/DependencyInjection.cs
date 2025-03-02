using Application.Builders;
using Application.CommandHandlers;
using Application.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //command handlers
        services.AddTransient<IApplicationUserCommandHandler, ApplicationUserCommandHandler>();
        services.AddTransient<IApplicationRoleCommandHandler, ApplicationRoleCommandHandler>();
        services.AddTransient<ISignInCommandHandler, SignInCommandHandler>();
        
        //builders
        services.AddTransient<IApplicationRoleBuilder, ApplicationRoleBuilder>();
        services.AddTransient<IApplicationUserBuilder, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //Decorators
        services.AddTransient<ISignInResultDecorator,SignInResultDecorator>();
        
        //query handlers
        return services;
    }
}
