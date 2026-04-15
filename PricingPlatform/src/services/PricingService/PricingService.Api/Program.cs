using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using OpenTelemetry.Metrics;
using PricingPlatform.Infrastructure.Logging.Extensions;
using PricingService.Api.Extensions;
using PricingService.Application.Interfaces;
using PricingService.Application.Validators;
using PricingService.Infrastructure.RuleProvider;
using PricingService.Infrastructure.Worker;

var builder = WebApplication.CreateBuilder(args);

// logging
builder.AddStructuredLogging();

// --------------------
// DI
// --------------------
builder.Services.AddHttpClient<IRuleProvider, RuleServiceClient>(c =>
{
    var baseUrl = builder.Configuration["RuleService:BaseUrl"];
    if (!string.IsNullOrWhiteSpace(baseUrl))
        c.BaseAddress = new Uri(baseUrl);
});

# region Observability (Metrics)
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();
        metrics.AddRuntimeInstrumentation();
    });
# endregion

// --------------------
// Rate Limiting
// --------------------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // endpoint policy
    options.AddFixedWindowLimiter("bulk-policy", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(10);
        opt.PermitLimit = 5;
    });

    // optional: global fallback (recommended for production)
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
    {
        var key = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: key,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            });
    });
});


//ValidatorsFrom
builder.Services.AddValidatorsFromAssemblyContaining<PriceRequestValidator>();

// --------------------
// Health + workers
// --------------------
builder.Services.AddHealthChecks();
builder.Services.AddHostedService<JobWorkerPool>();

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddPricingServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --------------------
// Middleware pipeline (IMPORTANT ORDER)
// --------------------
app.UseExceptionHandler("/error");

app.UseRouting();

app.UseRateLimiter();

app.UseApiDocs();
app.UseObservability();


app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapControllers();

await app.WarmupAsync();
app.Run();