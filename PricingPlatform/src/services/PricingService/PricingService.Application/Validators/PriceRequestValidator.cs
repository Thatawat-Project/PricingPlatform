using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using PricingPlatform.Contracts.DTOs;

namespace PricingService.Application.Validators
{
    public class PriceRequestValidator : AbstractValidator<QuoteRequest>
    {
        public PriceRequestValidator()
        {
            RuleFor(x => x.BasePrice)
                .GreaterThan(0);

            RuleFor(x => x.Weight)
                .InclusiveBetween(0, 1000);

            RuleFor(x => x.Zone)
                .NotEmpty();
        }
    }
}
