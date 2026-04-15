using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.Engine.Core;
using PricingService.Application.DTOs;
using PricingService.Application.Interfaces;
using PricingService.Application.UseCases;
using PricingService.UnitTests.Fakes;

namespace PricingService.UnitTests.Services
{
    public class QuotePricingServiceTests
    {
        private static PriceQuoteUseCase BuildService(IPipelineCache cache)
            => new(cache, NullLogger<PriceQuoteUseCase>.Instance);

        [Fact]
        public void Should_Return_Price_From_Cache()
        {
            var fakeCache = new FakePipelineCache
            {
                Pipeline = new PricingPipelineCompiler().Compile(new List<Rule>())
            };

            var result = BuildService(fakeCache).Price(new QuoteRequest { BasePrice = 100 });

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFallback);
            Assert.Equal(100, result.Value!.Price);
        }

        [Fact]
        public void Should_Return_Fallback_When_Cache_Empty()
        {
            var emptyCache = new FakePipelineCache { Pipeline = null };

            var result = BuildService(emptyCache).Price(new QuoteRequest { BasePrice = 100 });

            Assert.True(result.IsSuccess);
            Assert.True(result.IsFallback);
            Assert.Equal(100, result.Value!.Price);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public void Should_Return_Zero_When_BasePrice_Is_Zero()
        {
            var fakeCache = new FakePipelineCache
            {
                Pipeline = new PricingPipelineCompiler().Compile(new List<Rule>())
            };

            var result = BuildService(fakeCache).Price(new QuoteRequest { BasePrice = 0 });

            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value!.Price);
        }
    }
}
