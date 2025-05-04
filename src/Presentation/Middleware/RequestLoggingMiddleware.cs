namespace Presentation.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("Incoming request {Method} - {Path}", 
            context.Request.Method,
            context.Request.Path);
        
        await next(context);
        
        logger.LogInformation("Response: {context.Response.StatusCode}", context.Response.StatusCode);
    }
}
