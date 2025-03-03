using Application.Builders;
using Application.CommandHandlers;
using Application.Decorators;
using Application.Interfaces;
using Application.Mediators;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shared.Commands.ApplicationRole;
using Shared.Commands.ApplicationUser;
using Shared.Responses;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //command handlers
            //Create Handlers
        services.AddScoped<ICommandHandler<CreateRoleCommand, IdentityResult>,CreateRoleCommandHandler>();
        services.AddScoped<ICommandHandler<CreateUserCommand, IdentityResult>,CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<PasswordSignInCommand, PasswordSignInResponse>,PasswordSignInCommandHandler>();
            //query handlers
            
        //mediator
        services.AddScoped<ICommandMediator, CommandMediator>();
        
        //builders
        services.AddScoped<IBuilder<CreateRoleCommand, ApplicationRole>, ApplicationRoleBuilder>();
        services.AddScoped<IBuilder<CreateUserCommand,ApplicationUser>, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //Decorators
        services.AddTransient<ISignInResultDecorator,SignInResultDecorator>();
        
        return services;
    }
}
