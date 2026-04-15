using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Engine.Core;

namespace PricingService.Application.Interfaces
{
    public interface IRuleProvider
    {
        Task<IReadOnlyList<Rule>> GetRulesAsync(CancellationToken ct);
    }
}
