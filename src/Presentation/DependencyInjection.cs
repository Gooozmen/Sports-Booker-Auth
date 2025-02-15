using Presentation.Environments;
using Infrastructure.Options;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Add configuration options
        services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

        return services;
    }

    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        //Singletons
        services.AddSingleton<IEnvironmentValidator, EnvironmentValidator>();

        //Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
    public static IConfigurationBuilder AddDefaultConfiguration<T>(this IConfigurationBuilder configurationBuilder) where T : class
    {
        // Add appsettings.json
        configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        // Add user secrets
        configurationBuilder.AddUserSecrets<T>();

        return configurationBuilder;
    }

    public static IApplicationBuilder UsePresentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}