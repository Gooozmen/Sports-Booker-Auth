using CourtBooker.Auth.Application.Environments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CourtBooker.Auth.Infrastructure.Environments;

public class EnvironmentValidator(IWebHostEnvironment environment) : IEnvironmentValidator
{
    // Check if the current environment is Development
    public bool IsDevelopment()
    {
        return environment.IsDevelopment();
    }

    // Check if the current environment is Staging
    public bool IsStaging()
    {
        return environment.IsStaging();
    }

    // Check if the current environment is Production
    public bool IsProduction()
    {
        return environment.IsProduction();
    }

    // Check for a custom environment
    public bool IsEnvironment(string environmentName)
    {
        return environment.IsEnvironment(environmentName);
    }

    // Log or validate environment (example usage)
    public void LogEnvironment()
    {
        Console.WriteLine($"Current Environment: {environment.EnvironmentName}");
    }
}