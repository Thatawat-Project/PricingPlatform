using System;
using System.Collections.Generic;
using System.Text;
using PricingService.Domain.Entities;

namespace PricingService.Application.Interfaces
{
    public interface IJobQueue
    {
        ValueTask EnqueueAsync(PricingJob job, CancellationToken ct);
        IAsyncEnumerable<PricingJob> DequeueAllAsync(CancellationToken ct);
    }
}
