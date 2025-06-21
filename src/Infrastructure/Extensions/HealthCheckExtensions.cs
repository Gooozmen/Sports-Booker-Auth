using CourtBooker.Auth.Infrastructure.HealthChecks;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class HealthCheckExtensions
{
    internal static void SetupAppHealthChecks(this IServiceCollection services)
    {
        services.SetUpElasticHttpClient();
        
        services.AddHealthChecks()
            .AddNpgSql(
                services.GetConnectionString().Value.AuthDb,
                name: "postgresql",
                tags: new[] { "db", "sql" }
            )
            .AddCheck<ElasticsearchHealthCheck>(
                name: "elasticsearch",
                tags: new[] { "infra", "elastic", "logging" }
            )
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });
        
    }
    
}