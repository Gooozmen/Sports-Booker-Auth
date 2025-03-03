using System.Reflection;
using Application.Builders;
using Application.Factories;
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
        services.AddScoped<IApplicationRoleBuilder, ApplicationRoleBuilder>();
        services.AddScoped<IApplicationUserBuilder, ApplicationUserBuilder>();
        services.AddTransient<IHttpResponseBuilder, HttpResponseBuilder>();
        
        //factories
        services.AddTransient<IPasswordSignInResponseFactory, PasswordSignInResponseFactory>();
        
        
        return services;
    }
    
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        //Mediator => MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
