using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;

namespace JobService.Domain.Entities
{
    public enum JobStatus
    {
        Pending,
        Processing,
        Completed,
        Failed
    }

    public sealed class PricingJob
    {
        public string Id { get; }
        public JobStatus Status { get; private set; }
        public IReadOnlyList<QuoteRequest> Requests { get; }
        public List<QuoteResult>? Results { get; private set; }

        public DateTime CreatedAt { get; }
        public DateTime? CompletedAt { get; private set; }

        public PricingJob(string id, IReadOnlyList<QuoteRequest> requests)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Job id is required");

            if (requests == null || requests.Count == 0)
                throw new ArgumentException("Requests cannot be empty");

            Id = id;
            Requests = requests;
            Status = JobStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            if (Status != JobStatus.Pending)
                throw new InvalidOperationException("Invalid state transition");

            Status = JobStatus.Processing;
        }

        public void Complete(List<QuoteResult> results)
        {
            if (Status != JobStatus.Processing)
                throw new InvalidOperationException("Invalid state transition");

            Results = results ?? throw new ArgumentNullException(nameof(results));
            Status = JobStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Fail()
        {
            Status = JobStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }
    }
}
