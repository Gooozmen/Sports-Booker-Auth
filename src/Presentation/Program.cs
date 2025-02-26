using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Presentation;
using Presentation.Environments;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDefaultConfiguration<Program>(); //Load additional configuration before registering services.
builder.Services.ConfigureOptions(builder.Configuration); //Register configuration options so that strongly-typed settings can be injected.
builder.Services.AddControllers(); //Register controllers and API endpoints.

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices();

builder.Services.AddOpenApi();

var app = builder.Build();

var environmentValidator = app.Services.GetRequiredService<IEnvironmentValidator>();

//If in Development, use developer-friendly middleware.
await app.UseDevelopEnvironment(environmentValidator);

app.UseRouting();//Configure the middleware pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();//Map controllers to endpoints.

app.UsePresentation();//Use any custom presentation middleware.

await app.RunAsync();
