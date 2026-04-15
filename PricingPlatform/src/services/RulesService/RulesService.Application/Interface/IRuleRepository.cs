using System;
using System.Collections.Generic;
using System.Text;
using RulesService.Application.DTOs;
using RulesService.Domain.Entities;

namespace RulesService.Application.Interface
{
    public interface IRuleRepository
    {
        void Add(Rule rule);

        Rule? Get(Guid id);

        List<Rule> GetAll();
        Rule? GetById(Guid id);

        List<Rule> GetByRuleId(Guid ruleId);
        void SetActive(Guid ruleId, Guid activeRuleId);
    }
}
