using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class LoggingExtensions
{
    internal static void SetUpSerilogSink(this WebApplicationBuilder builder)
    {
        
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
    }
}