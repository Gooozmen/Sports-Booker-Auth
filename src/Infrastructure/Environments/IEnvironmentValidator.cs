namespace Infrastructure.Environments;

public interface IEnvironmentValidator
{
    bool IsDevelopment();
    bool IsStaging();
    bool IsProduction();
    bool IsEnvironment(string environmentName);
    void LogEnvironment();
}