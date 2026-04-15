using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.Contracts.Enums;

namespace PricingService.Domain.Entities
{
    public sealed class PricingJob
    {
        public string Id { get; }
        public JobStatus Status { get; private set; }

        public IReadOnlyList<QuoteRequest> Requests { get; }

        public IReadOnlyList<QuoteResult>? Results { get; private set; }

        public DateTime CreatedAtUtc { get; }
        public DateTime? StartedAtUtc { get; private set; }
        public DateTime? CompletedAtUtc { get; private set; }
        public string? Error { get; private set; }

        public PricingJob(string id, IReadOnlyList<QuoteRequest> requests)
        {
            Id = id;
            Requests = requests;
            Status = JobStatus.Pending;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void Start()
        {
            if (Status != JobStatus.Pending)
                throw new InvalidOperationException($"Invalid state: {Status}");

            Status = JobStatus.Processing;
            StartedAtUtc = DateTime.UtcNow;
        }

        public void Complete(List<QuoteResult> results)
        {
            if (Status != JobStatus.Processing)
                throw new InvalidOperationException($"Invalid state: {Status}");

            Results = results.ToArray();
            Status = JobStatus.Completed;
            CompletedAtUtc = DateTime.UtcNow;
        }

        public void Fail(string? error = null)
        {
            Status = JobStatus.Failed;
            Error = error;
            CompletedAtUtc = DateTime.UtcNow;
        }
    }
}
