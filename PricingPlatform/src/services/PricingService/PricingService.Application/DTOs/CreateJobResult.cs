using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.Enums;

namespace PricingService.Application.DTOs
{
    public sealed class CreateJobResult
    {
        public string JobId { get; init; } = default!;
        public string StatusUrl { get; init; } = default!;
        public int RequestCount { get; init; }
        public JobStatus Status { get; init; }
        public DateTime CreatedAtUtc { get; init; }
    }
}
