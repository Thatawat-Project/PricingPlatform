using System;
using System.Collections.Generic;
using System.Text;
using JobService.Application.Interfaces;
using JobService.Domain.Entities;
using PricingPlatform.Contracts.DTOs;

namespace JobService.Application.UseCases
{
    /// <summary>
    /// Create bulk pricing job and enqueue for processing
    /// </summary>
    public sealed class CreateJobUseCase
    {
        private readonly IJobRepository _repository;
        private readonly IJobQueue _queue;

        public CreateJobUseCase(
            IJobRepository repository,
            IJobQueue queue)
        {
            _repository = repository;
            _queue = queue;
        }

        public async Task<string> ExecuteAsync(
            IReadOnlyList<QuoteRequest> requests,
            CancellationToken ct)
        {
            if (requests == null || requests.Count == 0)
                throw new ArgumentException("Requests cannot be empty");

            var jobId = Guid.NewGuid().ToString("N");

            var job = new PricingJob(jobId, requests);

            // persist first (source of truth)
            _repository.Add(job);

            // enqueue for background processing
            await _queue.EnqueueAsync(job, ct);

            return jobId;
        }
    }
}
