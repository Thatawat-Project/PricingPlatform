using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Metrics;
using PricingPlatform.Infrastructure.Logging.Extensions;
using RulesService.Application.Interface;
using RulesService.Application.Services;
using RulesService.Infrastructure.HealthChecks;
using RulesService.Infrastructure.Repositories;
using RulesService.Infrastructure.Seed;
using RulesService.Infrastructure.Webhooks;

var builder = WebApplication.CreateBuilder(args);

builder.AddStructuredLogging();

# region Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
# endregion

# region HttpClient + Retry (outbound resilience)
builder.Services.AddHttpClient<IPricingServiceWebhook, PricingServiceWebhook>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["PricingService:BaseUrl"]!);
    c.Timeout = TimeSpan.FromSeconds(3);
});
# endregion

# region DI
builder.Services.AddSingleton<IRuleRepository, InMemoryRuleRepository>();
builder.Services.AddScoped<IRuleService, RuleService>();
# endregion

# region Observability (Metrics)
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();
        metrics.AddRuntimeInstrumentation();
    });
# endregion

# region HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck<RulesCoreHealthCheck>("rules-core", failureStatus: HealthStatus.Degraded)
    .AddCheck<RulesDependencyHealthCheck>("rules-webhook", failureStatus: HealthStatus.Degraded);
# endregion

# region Rate Limiting (inbound protection)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromSeconds(10),
                QueueLimit = 10,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));
});
# endregion

var app = builder.Build();

app.UseApiDocs();
app.UseObservability();

app.UseRouting();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true
});

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IRuleRepository>();
    var path = Path.Combine(app.Environment.ContentRootPath, "sample-data/rules.json");

    RuleSeeder.SeedFromFile(repo, path);
}

app.Run();