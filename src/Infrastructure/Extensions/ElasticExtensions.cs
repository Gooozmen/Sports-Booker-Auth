using CourtBooker.Auth.Infrastructure.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class ElasticExtensions
{
    internal static void SetUpElasticHttpClient(this IServiceCollection services)
    {
        // add a check for when the elastic instance is on cloud, we need to add auth to the client :(
        services.AddHttpClient<ElasticsearchHealthCheck>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:9200");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.elasticsearch+json;compatible-with=8");
        });
    }
}