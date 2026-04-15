using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PricingPlatform.Engine.Core;
using PricingService.IntegrationTests.Factories;
using PricingService.IntegrationTests.Models.DTOs;

namespace PricingService.IntegrationTests
{
    public class PricingApiTests : IClassFixture<TestFactory>
    {
        private readonly HttpClient _client;

        public PricingApiTests(TestFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Return_Price()
        {
            var response = await _client.PostAsJsonAsync(
                "/quotes/price",
                new PriceContext
                {
                    BasePrice = 150m,
                    Weight = 12m,
                    Zone = "B",
                    HourOfDay = 10,
                    DayOfWeek = DayOfWeek.Monday
                });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PriceResponse>();

            Assert.NotNull(result);
            Assert.Equal(236.5m, result!.Price);
        }
    }
}
