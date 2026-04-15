using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;
using PricingService.Application.Interfaces;

namespace PricingService.IntegrationTests.Fake
{
    public sealed class FakeRuleProvider : IRuleProvider
    {
        public Task<IReadOnlyList<Rule>> GetRulesAsync(CancellationToken ct)
        {
            var rules = new List<Rule>
        {
            // WeightTier → +5 (12kg)
            new Rule
            {
                Type = RuleType.WeightTier,
                WeightTier = new WeightTierRuleConfig
                {
                    Tiers = new List<WeightTierConfig>
                    {
                        new()
                        {
                            MinWeight = 10,
                            MaxWeight = null,
                            PricingType = WeightTierPricingType.PerKg,
                            Value = 5
                        }
                    }
                }
            },

            // TimeWindow → -20
            new Rule
            {
                Type = RuleType.TimeWindowPromotion,
                TimeWindow = new TimeWindowConfig
                {
                    StartHour = 0,
                    EndHour = 23,
                    DiscountType = DiscountType.FlatFee,
                    Value = 20
                }
            },

            // RemoteArea → +25
            new Rule
            {
                Type = RuleType.RemoteAreaSurcharge,
                RemoteArea = new RemoteAreaConfig
                {
                    Zones = new List<string> { "B" },
                    SurchargeType = SurchargeType.FlatFee,
                    Value = 25
                }
            },

            // RemoteArea → +10%
            new Rule
            {
                Type = RuleType.RemoteAreaSurcharge,
                RemoteArea = new RemoteAreaConfig
                {
                    Zones = new List<string> { "B" },
                    SurchargeType = SurchargeType.Percent,
                    Value = 10
                }
            }
        };

            return Task.FromResult<IReadOnlyList<Rule>>(rules);
        }
    }
}
