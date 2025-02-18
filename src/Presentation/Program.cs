using Application;
using Domain.Models;
using Infrastructure;
using Infrastructure.Database;
using Presentation;
using Presentation.Environments;

var builder = WebApplication.CreateBuilder(args);

// Configuration Options
builder.Services.ConfigureOptions(builder.Configuration);

// Register HttpClientFactory
builder.Services.AddHttpClient();

// Register controllers and API endpoints
builder.Services.AddControllers();

// Services Registration by Application Layer
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices();
builder.Services.AddOpenApi();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>();
builder.Configuration.AddDefaultConfiguration<Program>();

var app = builder.Build();

var environmentValidator = app.Services.GetRequiredService<IEnvironmentValidator>();

// Ensure correct middleware order
if (environmentValidator.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"); 
        options.RoutePrefix = string.Empty;
    });

    // Run database initializer asynchronously before starting the app
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
    }
}

// Middleware pipeline setup
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UsePresentation();

// Start the application
await app.RunAsync();