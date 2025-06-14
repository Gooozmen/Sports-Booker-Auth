using CourtBooker.Auth.Application;
using CourtBooker.Auth.Presentation;
using CourtBooker.Auth.Infrastructure;
using Serilog;

try{
    
    AppContext.SetSwitch("Elastic.Extensions.Logging.EmitDebug", true);
    AppContext.SetSwitch("Elastic.Extensions.Logging.EmitFailure", true);
    
    var builder = WebApplication.CreateBuilder(args);
        Console.WriteLine("Builder created");
        
    builder.SetupLoggingInfrastructure();
    Log.Information("Logging infrastructure set up");
    
    // Validar configuración antes de continuar
    ConfigValidator.ValidateRequiredConfiguration(builder.Configuration);
    
    builder.WebHost.UseUrls("http://0.0.0.0:80");
    Log.Information("URL Defined");

    builder.Configuration.AddDefaultConfiguration<Program>();
    Log.Information("Default configuration added");

    builder.Services.ConfigureOptions(builder.Configuration);
    Log.Information("Options configured");

    builder.Services.ConfigureJwt();
    Log.Information("JWT configured");

    builder.Services.AddInfrastructure();
    Log.Information("Infrastructure services added");

    builder.Services.AddApplicationServices();
    Log.Information("Application services added");

    builder.Services.AddPresentationServices();
    Log.Information("Presentation services added");

    var app = builder.Build();
    Log.Information("Application built");

    app.UsePresentationMiddlewares();

    await app.UseEnvironment();
    Log.Information("Environment set up");
        
    app.UseRouting(); //Configure the middleware pipeline.
    Log.Information("Routing set up");

    app.UseAuthentication();
    Log.Information("Authentication set up");

    app.UseAuthorization();
    Log.Information("Authorization set up");

    app.MapControllers(); //Map controllers to endpoints.
    Log.Information("Controllers mapped to endpoints");
        
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Error(ex.Message);
    throw;
}