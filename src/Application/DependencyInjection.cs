using Application.Builders;
using Application.CommandHandlers;
using Application.Decorators;
using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Commands;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //command handlers
            //Create Handlers
            //query handlers
            
        
        //builders
        services.AddScoped<IBuilder<CreateRoleCommand, ApplicationRole>, ApplicationRoleBuilder>();
        services.AddScoped<IBuilder<CreateUserCommand,ApplicationUser>, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //Decorators
        services.AddTransient<ISignInResultDecorator,SignInResultDecorator>();
        
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
