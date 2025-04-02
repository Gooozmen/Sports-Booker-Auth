using Application;
using Presentation;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDefaultConfiguration<Program>(); //Load additional configuration before registering services.
builder.Services.ConfigureOptions(builder.Configuration); //Register configuration options so that strongly-typed settings can be injected.
builder.Services.ConfigureJwt();

builder.SetupLoggingInfrastructure();
//architecture layers
builder.Services.AddInfrastructure();
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices();

var app = builder.Build();

await app.UseDevelopEnvironment();
app.UseRouting(); //Configure the middleware pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); //Map controllers to endpoints.

await app.RunAsync();