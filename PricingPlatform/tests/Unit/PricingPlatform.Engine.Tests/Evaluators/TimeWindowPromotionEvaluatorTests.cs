using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;
using PricingPlatform.Engine.Evaluators;

namespace PricingPlatform.Engine.Tests.Evaluators
{
    public class TimeWindowPromotionEvaluatorTests
    {
        [Fact]
        public void Should_Apply_FlatFee_Discount_When_In_TimeWindow()
        {
            var ctx = new PriceContext { BasePrice = 100, HourOfDay = 10, DayOfWeek = DayOfWeek.Monday };

            var rule = new Rule
            {
                TimeWindow = new TimeWindowConfig
                {
                    StartHour = 0,
                    EndHour = 23,
                    ApplicableDays = new List<DayOfWeek>(),
                    DiscountType = DiscountType.FlatFee,
                    Value = 20
                }
            };

            var effect = TimeWindowPromotionEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(80m, result);
        }

        [Fact]
        public void Should_Apply_Percent_Discount_When_In_TimeWindow()
        {
            var ctx = new PriceContext { BasePrice = 100, HourOfDay = 10, DayOfWeek = DayOfWeek.Monday };

            var rule = new Rule
            {
                TimeWindow = new TimeWindowConfig
                {
                    StartHour = 0,
                    EndHour = 23,
                    ApplicableDays = new List<DayOfWeek>(),
                    DiscountType = DiscountType.Percent,
                    Value = 20
                }
            };

            var effect = TimeWindowPromotionEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(80m, result);
        }

        [Fact]
        public void Should_Return_Same_Price_When_Outside_TimeWindow()
        {
            var ctx = new PriceContext { BasePrice = 100, HourOfDay = 15, DayOfWeek = DayOfWeek.Monday };

            var rule = new Rule
            {
                TimeWindow = new TimeWindowConfig
                {
                    StartHour = 10,
                    EndHour = 12,
                    ApplicableDays = new List<DayOfWeek>(),
                    DiscountType = DiscountType.FlatFee,
                    Value = 20
                }
            };

            var effect = TimeWindowPromotionEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(100m, result);
        }

        [Fact]
        public void Should_Return_Same_Price_When_Day_Not_Match()
        {
            var ctx = new PriceContext { BasePrice = 100, HourOfDay = 10, DayOfWeek = DayOfWeek.Sunday };

            var rule = new Rule
            {
                TimeWindow = new TimeWindowConfig
                {
                    StartHour = 0,
                    EndHour = 23,
                    ApplicableDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Friday },
                    DiscountType = DiscountType.FlatFee,
                    Value = 20
                }
            };

            var effect = TimeWindowPromotionEvaluator.Execute(in ctx, in rule);

            var result = (ctx.BasePrice + effect.Additive) * effect.Multiplicative;

            Assert.Equal(100m, result);
        }
    }
}
