using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PricingPlatform.Contracts.Utils;
using PricingPlatform.Engine.Configs;
using PricingPlatform.Engine.Core;
using PricingService.Application.DTOs;
using PricingService.Application.Interfaces;

namespace PricingService.Infrastructure.RuleProvider
{
    public sealed class RuleServiceClient : IRuleProvider
    {
        private readonly HttpClient _http;
        private readonly ILogger<RuleServiceClient> _logger;

        public RuleServiceClient(HttpClient http, ILogger<RuleServiceClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Rule>> GetRulesAsync(CancellationToken ct)
        {
            if (_http.BaseAddress is null)
            {
                _logger.LogWarning("RuleService BaseUrl not configured — skipping rule fetch");
                return Array.Empty<Rule>();
            }

            _logger.LogInformation("Fetching active rules from RuleService");

            List<RuleDto> dtos;

            try
            {
                dtos = await _http.GetFromJsonAsync<List<RuleDto>>("/rules/active", ct)
                       ?? new List<RuleDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to reach RuleService at {BaseAddress}", _http.BaseAddress);
                return Array.Empty<Rule>();
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                _logger.LogError(ex, "Request to RuleService timed out");
                return Array.Empty<Rule>();
            }

            if (dtos.Count == 0)
            {
                _logger.LogWarning("RuleService returned empty rule list");
                return Array.Empty<Rule>();
            }

            var filteredDtos = dtos
                .GroupBy(r => r.Type)
                .SelectMany(g => g.Key switch
                {
                    nameof(RuleType.WeightTier) or nameof(RuleType.TimeWindowPromotion)
                        => g.OrderByDescending(r => r.Priority).Take(1),
                    _ => g
                })
                .ToList();

            List<Rule> mapped;

            try
            {
                mapped = filteredDtos.Select(r =>
                {
                    var type = Enum.Parse<RuleType>(r.Type);

                    return new Rule
                    {
                        Type = type,
                        WeightTier = type == RuleType.WeightTier
                            ? JsonHelper.DeserializeFlexible<WeightTierRuleConfig>(r.ConfigJson)
                            : null,
                        RemoteArea = type == RuleType.RemoteAreaSurcharge
                            ? JsonHelper.DeserializeFlexible<RemoteAreaConfig>(r.ConfigJson)
                            : null,
                        TimeWindow = type == RuleType.TimeWindowPromotion
                            ? JsonHelper.DeserializeFlexible<TimeWindowConfig>(r.ConfigJson)
                            : null
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to map {Count} RuleDtos to domain rules", dtos.Count);
                return Array.Empty<Rule>();
            }

            _logger.LogInformation("Fetched {RuleCount} rules — types: {Types}",
                mapped.Count,
                mapped.Select(r => r.Type).Distinct());

            return mapped;
        }
    }
}
