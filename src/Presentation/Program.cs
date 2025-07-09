using CourtBooker.Auth.Application;
using CourtBooker.Auth.Presentation;
using CourtBooker.Auth.Infrastructure;
using Serilog;

try{
    var builder = WebApplication.CreateBuilder(args);
    Console.WriteLine("Builder created");
    
    builder.AddDefaultConfiguration();
    Console.WriteLine("Default configuration added");
    
    builder.Configuration.AddAzureKeyVault();
    Console.WriteLine("Azure Key Vault Added");

    builder.Services.AddConfigurationOptions(builder.Configuration);
    
    builder.AddLoggingInfrastructure();
    Log.Information("Logging infrastructure set up");
    
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80);
    });

    Log.Information("URL Defined");
    
    builder.Services.AddJwt();
    Log.Information("JWT configured");
    
    builder.Services.AddInfrastructure();
    Log.Information("Infrastructure services added");
    
    builder.Services.AddApplicationServices();
    Log.Information("Application services added");
    
    builder.Services.AddPresentationServices();
    Log.Information("Presentation services added");
    
    builder.Services.AddAppHealthChecks();
    Log.Information("Health checks configured");
    
    var app = builder.Build();
    Log.Information("Application built");
    
    app.UsePresentationMiddlewares();
    
    await app.UseEnvironment();
    Log.Information("Environment set up");
        
    app.UseRouting();
    Log.Information("Routing set up");
    
    app.UseAuthentication();
    Log.Information("Authentication set up");
    
    app.UseAuthorization();
    Log.Information("Authorization set up");
    
    app.MapControllers();
    app.MapAppHealthEndpoints();
    Log.Information("Controllers mapped to endpoints");
        
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"NOT WORKING {ex.Message}");
    Log.Fatal(ex, "Host terminated unexpectedly");  
    Log.Error(ex.Message);
    throw;
}