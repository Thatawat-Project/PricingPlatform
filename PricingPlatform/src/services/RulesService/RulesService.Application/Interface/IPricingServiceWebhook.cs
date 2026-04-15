using System;
using System.Collections.Generic;
using System.Text;

namespace RulesService.Application.Interface
{
    public interface IPricingServiceWebhook
    {
        Task<bool> NotifyRuleChangedAsync(CancellationToken ct);
        Task<bool> PingAsync(CancellationToken ct);
    }
}
