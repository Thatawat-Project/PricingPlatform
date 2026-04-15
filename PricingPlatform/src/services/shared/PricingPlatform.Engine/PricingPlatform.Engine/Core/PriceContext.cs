using System;
using System.Collections.Generic;
using System.Text;

namespace PricingPlatform.Engine.Core
{
    public readonly struct PriceContext
    {
        public decimal BasePrice { get; init; }
        public decimal Weight { get; init; }
        public string Zone { get; init; }
        public int HourOfDay { get; init; }
        public DayOfWeek DayOfWeek { get; init; }
    }
}
