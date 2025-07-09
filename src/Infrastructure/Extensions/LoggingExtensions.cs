using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace CourtBooker.Auth.Infrastructure.Extensions;

internal static class LoggingExtensions
{
    internal static void SetUpSerilogSink(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            // Try grab the first node from configuration:
            var firstNode = context.Configuration["Serilog:WriteTo:1:Args:nodes:0"];
            Console.WriteLine($"Elastic Node: {firstNode}");

            if (string.IsNullOrWhiteSpace(firstNode))
            {
                // No valid ES node, so just log to Console
                configuration
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            }
            else
            {
                // Node present, let Serilog.Settings.Configuration wire up all sinks
                configuration
                    .ReadFrom.Configuration(context.Configuration);
            }
        });
    }
}