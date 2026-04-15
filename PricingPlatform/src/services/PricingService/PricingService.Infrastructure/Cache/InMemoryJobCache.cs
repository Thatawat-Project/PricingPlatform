using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using PricingService.Application.Interfaces;
using PricingService.Domain.Entities;

namespace PricingService.Infrastructure.Cache
{
    public sealed class InMemoryJobCache : IJobCache
    {
        private readonly ConcurrentDictionary<string, PricingJob> _jobs = new();

        public void Add(PricingJob job)
            => _jobs.TryAdd(job.Id, job);

        public bool TryGet(string jobId, out PricingJob job)
            => _jobs.TryGetValue(jobId, out job!);

        public void Update(PricingJob job)
            => _jobs[job.Id] = job;
    }
}
