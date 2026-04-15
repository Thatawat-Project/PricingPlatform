using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.Enums;

namespace RulesService.Domain.Entities
{
    public sealed class Rule
    {
        public Guid Id { get; set; }
        public Guid RuleId { get; set; }
        public RuleType Type { get; set; }

        public string ConfigJson { get; set; } = default!;

        public int Priority { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
    }
}
