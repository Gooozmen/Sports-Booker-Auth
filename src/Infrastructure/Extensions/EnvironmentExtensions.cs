using CourtBooker.Auth.Application.Environments;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class EnvironmentExtensions
{
    internal static async Task SetupApplicationEnvironment(this WebApplication app)
    {
        var environmentValidator = app.Services.GetRequiredService<IEnvironmentValidator>();
        if (environmentValidator.IsDevelopment()) 
            await SetUpDevelopmentEnvironment(app);
        if (environmentValidator.IsStaging())
            await SetUpDevelopmentEnvironment(app);
    }
    
    private static async Task SetUpDevelopmentEnvironment(this WebApplication app)
    {
        app.UseDeveloperExceptionPage();
        await app.RunDatabaseInitialization();
    }
}