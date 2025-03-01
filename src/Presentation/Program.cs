using Application;
using Infrastructure;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Presentation;
using Presentation.Environments;
using Presentation.Interceptors;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDefaultConfiguration<Program>(); //Load additional configuration before registering services.
builder.Services.ConfigureOptions(builder.Configuration); //Register configuration options so that strongly-typed settings can be injected.
builder.Services.ConfigureJwt();

builder.Services.AddControllers(o => { o.Filters.Add<ModelStateInterceptor>(); });//Register controllers and API endpoints.
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

app.MapSwagger().AllowAnonymous();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root.
});

app.MapControllers();//Map controllers to endpoints.
app.UsePresentation();//Use any custom presentation middleware.

await app.RunAsync();
