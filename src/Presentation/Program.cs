using Application;
using Infrastructure;
using Infrastructure.Database;
using Presentation;
using Presentation.Environments;

var builder = WebApplication.CreateBuilder(args);

// Configuration Options
builder.Services.ConfigureOptions(builder.Configuration);

// Register HttpClientFactory
builder.Services.AddHttpClient();

// Registers controllers and API endpoints
builder.Services.AddControllers();

// Services Registration by Application Layer
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices();
builder.Configuration.AddDefaultConfiguration<Program>();

var app = builder.Build();

var environmentValidation = app.Services.GetRequiredService<IEnvironmentValidator>().IsDevelopment();

// Configure middleware pipeline
app.UseStaticFiles(); // Serve static files (should come early to serve files directly)
app.UseRouting(); // Enable routing for middleware and endpoints
app.MapControllers(); // Map controllers to endpoints
app.UsePresentation();

if (environmentValidation)
{
    app.UseDeveloperExceptionPage();
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
    }
}
// Start the application
app.Run();