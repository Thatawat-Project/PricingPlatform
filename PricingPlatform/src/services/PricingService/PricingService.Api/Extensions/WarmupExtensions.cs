using PricingService.Application.Handlers;

namespace PricingService.Api.Extensions
{
    public static class WarmupExtensions
    {
        public static async Task WarmupAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<RulePublishedHandler>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            try
            {
                await handler.HandleAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Warmup failed — PricingService will start with empty pipeline");
            }
        }
    }
}
