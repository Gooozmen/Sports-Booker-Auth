using Application.Builders;
using Application.CommandHandlers;
using Application.Decorators;
using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Commands.ApplicationRole;
using Shared.Commands.ApplicationUser;
using Shared.Interfaces;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //command handlers
        services.AddTransient<IApplicationUserCommandHandler, ApplicationUserCommandHandler>();
        services.AddTransient<ICommandHandler<ICommand,object>, ApplicationRoleCommandHandler>();
        services.AddTransient<ISignInCommandHandler, SignInCommandHandler>();
        
        //builders
        services.AddScoped<IBuilder<CreateRoleCommand, ApplicationRole>, ApplicationRoleBuilder>();
        services.AddScoped<IBuilder<CreateUserCommand,ApplicationUser>, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //Decorators
        services.AddTransient<ISignInResultDecorator,SignInResultDecorator>();
        
        //query handlers
        return services;
    }
}
