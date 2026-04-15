using System;
using System.Collections.Generic;
using System.Text;

namespace PricingPlatform.Contracts.DTOs
{
    public sealed class QuoteRequest
    {
        public decimal BasePrice { get; set; }
        public decimal Weight { get; set; }
        public string Zone { get; set; } = string.Empty;
    }
}
