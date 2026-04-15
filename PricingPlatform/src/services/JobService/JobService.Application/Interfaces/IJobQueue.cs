using System;
using System.Collections.Generic;
using System.Text;
using JobService.Domain.Entities;

namespace JobService.Application.Interfaces
{
    public interface IJobQueue
    {
        /// <summary>
        /// Enqueue job for background processing
        /// </summary>
        ValueTask EnqueueAsync(PricingJob job, CancellationToken ct);

        /// <summary>
        /// Stream jobs for worker consumption
        /// </summary>
        IAsyncEnumerable<PricingJob> DequeueAllAsync(CancellationToken ct);
    }
}
