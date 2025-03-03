namespace Presentation.Environments;

public class EnvironmentValidator : IEnvironmentValidator
{
    private readonly IWebHostEnvironment _environment;

    public EnvironmentValidator(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    // Check if the current environment is Development
    public bool IsDevelopment()
    {
        return _environment.IsDevelopment();
    }

    // Check if the current environment is Staging
    public bool IsStaging()
    {
        return _environment.IsStaging();
    }

    // Check if the current environment is Production
    public bool IsProduction()
    {
        return _environment.IsProduction();
    }

    // Check for a custom environment
    public bool IsEnvironment(string environmentName)
    {
        return _environment.IsEnvironment(environmentName);
    }

    // Log or validate environment (example usage)
    public void LogEnvironment()
    {
        Console.WriteLine($"Current Environment: {_environment.EnvironmentName}");
    }
}