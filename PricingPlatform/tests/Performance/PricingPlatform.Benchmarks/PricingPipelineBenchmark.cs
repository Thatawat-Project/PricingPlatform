using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using BenchmarkDotNet.Attributes;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;

namespace PricingPlatform.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 5)]
    public class PricingPipelineBenchmark
    {
        private CompiledPricingPipeline _pipeline;
        private PriceContext _context;

        [GlobalSetup]
        public void Setup()
        {
            var compiler = new PricingPipelineCompiler();

            var rules = new List<Rule>
    {
        new Rule
        {
            Type = RuleType.WeightTier,
            WeightTier = new WeightTierRuleConfig
            {
                Tiers = new List<WeightTierConfig>
                {
                    new WeightTierConfig
                    {
                        MinWeight = 0,
                        MaxWeight = 10,
                        PricingType = WeightTierPricingType.FlatFee,
                        Value = 50m
                    }
                }
            }
        },
        new Rule
        {
            Type = RuleType.RemoteAreaSurcharge,
            RemoteArea = new RemoteAreaConfig
            {
                Zones = new List<string> { "A" },
                SurchargeType = SurchargeType.FlatFee,
                Value = 30m
            }
        },
        new Rule
        {
            Type = RuleType.TimeWindowPromotion,
            TimeWindow = new TimeWindowConfig
            {
                StartHour = 0,
                EndHour = 23,
                ApplicableDays = new List<DayOfWeek>(),
                DiscountType = DiscountType.FlatFee,
                Value = 20m
            }
        }
    };

            _pipeline = compiler.Compile(rules);

            _context = new PriceContext
            {
                BasePrice = 100,
                Weight = 5,
                Zone = "A",
                HourOfDay = 10,
                DayOfWeek = DayOfWeek.Monday
            };
        }

        [Benchmark]
        public decimal ExecutePipeline()
        {
            return _pipeline.Execute(_context);
        }
    }
}
