using Serilog.Context;

namespace CourtBooker.Auth.Presentation.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var id)
            ? id.ToString()
            : Guid.NewGuid().ToString();

        context.Items[CorrelationIdHeader] = correlationId;

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }
}