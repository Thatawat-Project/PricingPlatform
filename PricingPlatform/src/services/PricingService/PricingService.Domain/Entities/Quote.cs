using System;
using System.Collections.Generic;
using System.Text;

namespace PricingService.Domain.Entities
{
    public sealed class Quote
    {
        public decimal BasePrice { get; init; }
        public decimal FinalPrice { get; private set; }

        public void ApplyPrice(decimal price)
        {
            FinalPrice = price;
        }
    }
}
