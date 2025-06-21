using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace CourtBooker.Auth.Infrastructure.HealthChecks;

public class ElasticsearchHealthCheck(HttpClient httpClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync
    (
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var response = await httpClient.GetAsync("", cancellationToken);
            return response.IsSuccessStatusCode ? 
                HealthCheckResult.Healthy("Elasticsearch is reachable") : 
                HealthCheckResult.Unhealthy($"Status code: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Exception while pinging Elasticsearch", ex);
        }
    }
}
