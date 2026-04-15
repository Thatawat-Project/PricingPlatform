using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.Contracts.Enums;

namespace PricingService.Application.DTOs
{
    public sealed class JobResult
    {
        public string JobId { get; init; } = default!;
        public JobStatus Status { get; init; }

        public int TotalRequests { get; init; }
        public int? CompletedRequests { get; init; }

        public IReadOnlyList<QuoteResult>? Results { get; init; }

        public string? Error { get; init; }

        public DateTime CreatedAtUtc { get; init; }
        public DateTime? CompletedAtUtc { get; init; }
    }
}
