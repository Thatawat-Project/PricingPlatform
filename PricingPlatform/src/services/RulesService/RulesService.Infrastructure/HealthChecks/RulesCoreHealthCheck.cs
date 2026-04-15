using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RulesService.Application.Interface;

namespace RulesService.Infrastructure.HealthChecks
{
    public sealed class RulesCoreHealthCheck : IHealthCheck
    {
        private readonly IRuleRepository _repo;

        public RulesCoreHealthCheck(IRuleRepository repo)
        {
            _repo = repo;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var rules = _repo.GetAll();

            if (rules.Count == 0)
                return Task.FromResult(HealthCheckResult.Degraded("No rules loaded"));

            if (!rules.Any(r => r.IsActive))
                return Task.FromResult(HealthCheckResult.Degraded("No active rules"));

            return Task.FromResult(HealthCheckResult.Healthy("Rules OK"));
        }
    }
}
