using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Engine.Configs;

namespace PricingPlatform.Engine.Core
{
    public struct Rule
    {
        public RuleType Type { get; init; }

        public WeightTierRuleConfig WeightTier;
        public RemoteAreaConfig RemoteArea;
        public TimeWindowConfig TimeWindow;
    }
}
