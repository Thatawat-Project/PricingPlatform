using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace PricingPlatform.Engine.Configs
{
    public enum WeightTierPricingType
    {
        FlatFee,
        PerKg
    }

    public class WeightTierRuleConfig
    {
        public List<WeightTierConfig> Tiers { get; init; } = new();
    }

    public class WeightTierConfig
    {
        public decimal MinWeight { get; init; }
        public decimal? MaxWeight { get; init; }
        public WeightTierPricingType PricingType { get; init; }
        public decimal Value { get; init; }
    }
}
