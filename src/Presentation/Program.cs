using System.Diagnostics;
using CourtBooker.Auth.Application;
using CourtBooker.Auth.Presentation;
using CourtBooker.Auth.Infrastructure;
using Serilog;

try{
    
    AppContext.SetSwitch("Elastic.Extensions.Logging.EmitDebug", true);
    AppContext.SetSwitch("Elastic.Extensions.Logging.EmitFailure", true);

    Console.WriteLine("Starting up");
    
    var builder = WebApplication.CreateBuilder(args);
        Console.WriteLine("Builder created");
        
    builder.SetupLoggingInfrastructure();
        Console.WriteLine("Logging infrastructure set up");
        
    Console.WriteLine("SINK ACTUAL: " + builder.Configuration["Serilog:WriteTo:0:Name"]);
    
    builder.WebHost.UseUrls("http://0.0.0.0:80");
        Console.WriteLine("URL Defined");

    builder.Configuration.AddDefaultConfiguration<Program>();
        Console.WriteLine("Default configuration added");

    builder.Services.ConfigureOptions(builder.Configuration);
        Console.WriteLine("Options configured");

    builder.Services.ConfigureJwt();
        Console.WriteLine("JWT configured");

    builder.Services.AddInfrastructure();
        Console.WriteLine("Infrastructure services added");

    builder.Services.AddApplicationServices();
        Console.WriteLine("Application services added");

    builder.Services.AddPresentationServices();
        Console.WriteLine("Presentation services added");

    var app = builder.Build();
        Console.WriteLine("Application built");

    app.UsePresentationMiddlewares();

    await app.UseEnvironment();
        Console.WriteLine("Environment set up");
        
    app.UseRouting(); //Configure the middleware pipeline.
        Console.WriteLine("Routing set up");

    app.UseAuthentication();
        Console.WriteLine("Authentication set up");

    app.UseAuthorization();
        Console.WriteLine("Authorization set up");

    app.MapControllers(); //Map controllers to endpoints.
        Console.WriteLine("Controllers mapped to endpoints");
        
    Log.Information("Prueba directa a Elastic Cloud desde Auth API");
        
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}