using System;
using System.Collections.Generic;
using System.Text;
using RulesService.Application.DTOs;

namespace RulesService.Application.Interface
{
    public interface IRuleService
    {
        Guid Create(CreateRuleRequest request);
        Task PublishAsync(Guid id, CancellationToken ct);
        Guid UpdateCreateVersion(Guid id, UpdateRuleRequest request);
        List<RuleDto> GetActive();
    }
}
