using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.Enums;

namespace RulesService.Application.DTOs
{
    public sealed class CreateRuleRequest
    {
        public RuleType Type { get; set; }
        public string ConfigJson { get; set; } = default!;
        public int Priority { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
