using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using RulesService.Application.DTOs;
using RulesService.Application.Interface;
using RulesService.Domain.Entities;

namespace RulesService.Application.Services
{
    public sealed class RuleService : IRuleService
    {
        private readonly IRuleRepository _repo;
        private readonly IPricingServiceWebhook _webhook;

        public RuleService(IRuleRepository repo, IPricingServiceWebhook webhook)
        {
            _repo = repo;
            _webhook = webhook;
        }

        public Guid Create(CreateRuleRequest request)
        {
            JsonDocument.Parse(request.ConfigJson);

            var rule = new Rule
            {
                Id = Guid.NewGuid(),
                RuleId = Guid.NewGuid(),
                Type = request.Type,
                ConfigJson = request.ConfigJson,
                Priority = request.Priority,
                EffectiveFrom = request.EffectiveFrom,
                IsActive = false,
                Version = 1
            };

            _repo.Add(rule);

            return rule.Id;
        }

        public Guid UpdateCreateVersion(Guid id, UpdateRuleRequest request)
        {
            var latest = _repo.GetById(id)
                ?? throw new KeyNotFoundException();

            JsonDocument.Parse(request.ConfigJson);

            var newRule = new Rule
            {
                Id = Guid.NewGuid(),
                RuleId = latest.RuleId,
                Type = request.Type,
                ConfigJson = request.ConfigJson,
                Priority = request.Priority,
                EffectiveFrom = request.EffectiveFrom,
                EffectiveTo = request.EffectiveTo,
                Version = latest.Version + 1,
                IsActive = false
            };

            _repo.Add(newRule);

            return newRule.Id;
        }

        public async Task PublishAsync(Guid id, CancellationToken ct)
        {
            var latest = _repo.GetById(id)
                ?? throw new KeyNotFoundException("Rule not found");

            var all = _repo.GetAll()
                .Where(r => r.RuleId == latest.RuleId)
                .ToList();

            foreach (var r in all)
                r.IsActive = false;

            latest.IsActive = true;

            await _webhook.NotifyRuleChangedAsync(ct);
        }

        public List<RuleDto> GetActive()
        {
            var now = DateTime.UtcNow;

            return _repo.GetAll()
                .Where(r =>
                    r.IsActive &&
                    r.EffectiveFrom <= now &&
                    (r.EffectiveTo == null || r.EffectiveTo >= now))
                .OrderByDescending(r => r.Priority)
                .Select(r => new RuleDto
                {
                    Id = r.Id,
                    Type = r.Type.ToString(),
                    Priority = r.Priority,
                    Version = (int)r.Version,
                    ConfigJson = r.ConfigJson
                })
                .ToList();
        }
    }
}
