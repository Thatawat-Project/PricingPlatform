using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using RulesService.Application.Interface;
using RulesService.Domain.Entities;

namespace RulesService.Infrastructure.Repositories
{
    public sealed class InMemoryRuleRepository : IRuleRepository
    {
        private readonly ConcurrentDictionary<Guid, Rule> _store = new();

        public void Add(Rule rule)
        {
            _store[rule.Id] = rule;
        }

        public Rule? Get(Guid id)
        {
            _store.TryGetValue(id, out var rule);
            return rule;
        }

        public List<Rule> GetAll()
            => _store.Values.ToList();

        public Rule? GetById(Guid id)
        {
            return _store.Values
                .Where(r => r.Id == id).FirstOrDefault();
        }

        public List<Rule> GetByRuleId(Guid ruleId)
        {
            return _store.Values
                .Where(r => r.RuleId == ruleId)
                .ToList();
        }

        public void SetActive(Guid ruleId, Guid activeRuleId)
        {
            foreach (var rule in _store.Values.Where(r => r.RuleId == ruleId))
            {
                rule.IsActive = rule.Id == activeRuleId;
            }
        }
    }
}
