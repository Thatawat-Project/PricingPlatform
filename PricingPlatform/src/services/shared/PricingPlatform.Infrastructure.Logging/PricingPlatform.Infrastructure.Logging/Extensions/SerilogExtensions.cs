using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace PricingPlatform.Infrastructure.Logging.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostApplicationBuilder AddStructuredLogging(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSerilog((ctx, config) => config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console(new CompactJsonFormatter())
                .WriteTo.File(
                    path: "logs/pricing-.log",
                    rollingInterval: RollingInterval.Day,
                    formatter: new CompactJsonFormatter()));

            return builder;
        }
    }
}
