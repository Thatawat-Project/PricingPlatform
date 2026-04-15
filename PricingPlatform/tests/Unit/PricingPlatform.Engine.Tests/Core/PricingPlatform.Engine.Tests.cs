using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;

namespace PricingPlatform.Engine.Tests.Core
{
    public class PricingPipelineTests
    {
        private readonly PricingPipelineCompiler _compiler = new();

        [Fact]
        public void Should_Return_BasePrice_When_No_Rules()
        {
            var pipeline = _compiler.Compile([]);

            var result = pipeline.Execute(new PriceContext
            {
                BasePrice = 100
            });

            Assert.Equal(100m, result);
        }

        [Fact]
        public void Should_Apply_Multiple_Rules()
        {
            var rules = new List<Rule>
            {
                // +50
                new Rule { Type = RuleType.WeightTier, WeightTier = new WeightTierRuleConfig
                {
                    Tiers = new() { new() { MinWeight = 0, MaxWeight = 10, PricingType = WeightTierPricingType.FlatFee, Value = 50 } }
                }},

                // +30
                new Rule { Type = RuleType.RemoteAreaSurcharge, RemoteArea = new RemoteAreaConfig
                {
                    Zones = new() { "A" },
                    SurchargeType = SurchargeType.FlatFee,
                    Value = 30
                }},

                // -20
                new Rule { Type = RuleType.TimeWindowPromotion, TimeWindow = new TimeWindowConfig
                {
                    StartHour = 0,
                    EndHour = 23,
                    ApplicableDays = new(),
                    DiscountType = DiscountType.FlatFee,
                    Value = 20
                }}
            };

            var pipeline = _compiler.Compile(rules);

            var result = pipeline.Execute(new PriceContext
            {
                BasePrice = 100,
                Weight = 5,
                Zone = "A",
                HourOfDay = 10,
                DayOfWeek = DayOfWeek.Monday
            });

            Assert.Equal(160m, result);
        }

        [Fact]
        public void Should_Be_Deterministic_Ignoring_Input_Order()
        {
            var rules = new List<Rule>
            {
                new Rule { Type = RuleType.TimeWindowPromotion, TimeWindow = new() { DiscountType = DiscountType.FlatFee, Value = 20, StartHour = 0, EndHour = 23 } },
                new Rule { Type = RuleType.WeightTier, WeightTier = new() { Tiers = new() { new() { MinWeight = 0, MaxWeight = 10, PricingType = WeightTierPricingType.FlatFee, Value = 50 } } } }
            };

            var pipeline = _compiler.Compile(rules);

            var result = pipeline.Execute(new PriceContext
            {
                BasePrice = 100,
                Weight = 5,
                HourOfDay = 10
            });

            Assert.Equal(130m, result);
        }
    }
}
