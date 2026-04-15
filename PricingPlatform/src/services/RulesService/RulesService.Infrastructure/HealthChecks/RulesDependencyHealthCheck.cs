using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RulesService.Application.Interface;

namespace RulesService.Infrastructure.HealthChecks
{
    public sealed class RulesDependencyHealthCheck : IHealthCheck
    {
        private readonly IPricingServiceWebhook _webhook;

        public RulesDependencyHealthCheck(IPricingServiceWebhook webhook)
        {
            _webhook = webhook;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var ok = await _webhook.PingAsync(cancellationToken);

                if (!ok)
                    return HealthCheckResult.Degraded("PricingService unreachable");

                return HealthCheckResult.Healthy("Dependency OK");
            }   
            catch
            {
                return HealthCheckResult.Degraded("PricingService error");
            }
        }
    }
}
