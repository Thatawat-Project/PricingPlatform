using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace PricingPlatform.Engine.Configs
{
    public enum SurchargeType
    {
        FlatFee,
        Percent
    }
    public class RemoteAreaConfig
    {
        public List<string> Zones { get; init; } = new();
        public SurchargeType SurchargeType { get; init; }
        public decimal Value { get; init; }
    }

}
