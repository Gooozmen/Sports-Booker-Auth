using Serilog.Context;

namespace CourtBooker.Auth.Presentation.Middleware;

public class HealthCheckTaggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/health"))
        {
            LogContext.PushProperty("RequestType", "HealthCheck");
        }

        await next(context);
    }
}


