using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;
using PricingPlatform.Engine.Evaluators;

namespace PricingPlatform.Engine.Tests.Evaluators
{
    public class RemoteAreaSurchargeEvaluatorTests
    {
        [Fact]
        public void Should_Add_FlatFee_When_Zone_Match()
        {
            var ctx = new PriceContext { BasePrice = 100, Zone = "A" };

            var rule = new Rule
            {
                RemoteArea = new RemoteAreaConfig
                {
                    Zones = new List<string> { "A" },
                    SurchargeType = SurchargeType.FlatFee,
                    Value = 30
                }
            };

            var effect = RemoteAreaSurchargeEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(130m, result);
        }

        [Fact]
        public void Should_Add_Percent_When_Zone_Match()
        {
            var ctx = new PriceContext { BasePrice = 100, Zone = "A" };

            var rule = new Rule
            {
                RemoteArea = new RemoteAreaConfig
                {
                    Zones = new List<string> { "A" },
                    SurchargeType = SurchargeType.Percent,
                    Value = 20
                }
            };

            var effect = RemoteAreaSurchargeEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(120m, result);
        }

        [Fact]
        public void Should_Return_Same_Price_When_Zone_Not_Match()
        {
            var ctx = new PriceContext { BasePrice = 100, Zone = "B" };

            var rule = new Rule
            {
                RemoteArea = new RemoteAreaConfig
                {
                    Zones = new List<string> { "A" },
                    SurchargeType = SurchargeType.FlatFee,
                    Value = 30
                }
            };

            var effect = RemoteAreaSurchargeEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(100m, result);
        }
    }
}
