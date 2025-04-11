using System.Reflection;
using Application.Behaviors;
using Application.Builders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //DI's
        services.AddScoped<IApplicationRoleBuilder, ApplicationRoleBuilder>();
        services.AddScoped<IApplicationUserBuilder, ApplicationUserBuilder>();
        services.AddScoped<IHttpResponseBuilder, HttpResponseBuilder>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        
        AddMediatR(services);

        return services;
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        //Mediator => MediatR
        services.AddMediatR(config => { config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()); });
    }
}