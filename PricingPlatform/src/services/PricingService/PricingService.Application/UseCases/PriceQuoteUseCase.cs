using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.Engine.Core;
using PricingPlatform.SharedKernel.Result;
using PricingService.Application.Interfaces;

namespace PricingService.Application.UseCases
{
    public sealed class PriceQuoteUseCase : IPriceQuoteUseCase
    {
        private readonly IPipelineCache _cache;
        private readonly ILogger<PriceQuoteUseCase> _logger;

        public PriceQuoteUseCase(IPipelineCache cache, ILogger<PriceQuoteUseCase> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public Result<QuoteResult> Price(QuoteRequest request)
        {
            if (!_cache.TryGet(out var pipeline))
            {
                _logger.LogWarning("Pipeline not yet initialized — returning BasePrice");
                return Result<QuoteResult>.Fallback(
                    new QuoteResult { Price = request.BasePrice },
                    "Pricing rules not yet loaded");
            }

            var now = DateTime.UtcNow;

            var ctx = new PriceContext
            {
                BasePrice = request.BasePrice,
                Weight = request.Weight,
                Zone = request.Zone,
                HourOfDay = now.Hour,
                DayOfWeek = now.DayOfWeek
            };

            var sw = Stopwatch.GetTimestamp();
            var finalPrice = pipeline.Execute(ctx);
            var elapsed = Stopwatch.GetElapsedTime(sw);

            _logger.LogInformation("Quote priced {FinalPrice} in {ElapsedMs}ms {@Context}",
                finalPrice, elapsed.TotalMilliseconds, ctx);

            return Result<QuoteResult>.Success(new QuoteResult { Price = finalPrice });
        }
    }
}
