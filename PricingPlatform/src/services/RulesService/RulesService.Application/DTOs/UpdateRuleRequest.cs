using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.Enums;

namespace RulesService.Application.DTOs
{
    public sealed class UpdateRuleRequest
    {
        public RuleType Type { get; init; }
        public decimal Value { get; init; }
        public string ConfigJson { get; init; } = string.Empty;
        public int Priority { get; init; }
        public DateTime EffectiveFrom { get; init; }
        public DateTime? EffectiveTo { get; init; }
    }
}
