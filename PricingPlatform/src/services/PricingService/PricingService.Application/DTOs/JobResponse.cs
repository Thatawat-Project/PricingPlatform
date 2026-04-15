using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;

namespace PricingService.Application.DTOs
{
    public sealed class JobResponse
    {
        public string JobId { get; init; }
        public string Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? CompletedAt { get; init; }
        public List<QuoteResult>? Results { get; init; }
    }
}
