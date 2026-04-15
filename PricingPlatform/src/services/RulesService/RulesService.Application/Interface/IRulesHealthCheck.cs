using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RulesService.Application.Interface
{
    public interface IRulesHealthCheck
    {
        Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default);
    }
}
