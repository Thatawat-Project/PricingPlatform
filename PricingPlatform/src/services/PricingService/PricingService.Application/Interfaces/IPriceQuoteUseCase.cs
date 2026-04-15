using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.SharedKernel.Result;

namespace PricingService.Application.Interfaces
{
    public interface IPriceQuoteUseCase
    {
        Result<QuoteResult> Price(QuoteRequest request);
    }
}
