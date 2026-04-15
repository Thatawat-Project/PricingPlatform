using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;
using PricingPlatform.Engine.Evaluators;

namespace PricingPlatform.Engine.Tests.Evaluators
{
    public class WeightTierEvaluatorTests
    {
        [Fact]
        public void Should_Return_FlatFee_When_Weight_In_Tier()
        {
            var ctx = new PriceContext { BasePrice = 100, Weight = 5 };

            var rule = new Rule
            {
                WeightTier = new WeightTierRuleConfig
                {
                    Tiers = new List<WeightTierConfig>
            {
                new() { MinWeight = 0, MaxWeight = 10, PricingType = WeightTierPricingType.FlatFee, Value = 50 }
            }
                }
            };

            var effect = WeightTierEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(50m, effect.Additive);
        }

        [Fact]
        public void Should_Return_PerKg_Price_When_Weight_In_Tier()
        {
            var ctx = new PriceContext { BasePrice = 100, Weight = 5 };

            var rule = new Rule
            {
                WeightTier = new WeightTierRuleConfig
                {
                    Tiers = new List<WeightTierConfig>
            {
                new() { MinWeight = 0, MaxWeight = 10, PricingType = WeightTierPricingType.PerKg, Value = 20 }
            }
                }
            };

            var effect = WeightTierEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(200m, result);
        }

        [Fact]
        public void Should_Return_Same_Price_When_Weight_Not_In_Any_Tier()
        {
            var ctx = new PriceContext { BasePrice = 100, Weight = 20 };

            var rule = new Rule
            {
                WeightTier = new WeightTierRuleConfig
                {
                    Tiers = new List<WeightTierConfig>
            {
                new() { MinWeight = 0, MaxWeight = 10, PricingType = WeightTierPricingType.FlatFee, Value = 50 }
            }
                }
            };

            var effect = WeightTierEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(100m, result);
        }

        [Fact]
        public void Should_Match_Last_Tier_When_MaxWeight_Is_Null()
        {
            var ctx = new PriceContext { BasePrice = 100, Weight = 25 };

            var rule = new Rule
            {
                WeightTier = new WeightTierRuleConfig
                {
                    Tiers = new List<WeightTierConfig>
            {
                new() { MinWeight = 0, MaxWeight = 10, PricingType = WeightTierPricingType.FlatFee, Value = 50 },
                new() { MinWeight = 10, MaxWeight = 20, PricingType = WeightTierPricingType.FlatFee, Value = 100 },
                new() { MinWeight = 20, MaxWeight = null, PricingType = WeightTierPricingType.PerKg, Value = 8 }
            }
                }
            };

            var effect = WeightTierEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(300m, result);
        }
    }
}
