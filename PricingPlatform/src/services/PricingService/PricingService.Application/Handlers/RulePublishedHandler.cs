using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using PricingPlatform.Engine.Core;
using PricingService.Application.Interfaces;

namespace PricingService.Application.Handlers
{
    public sealed class RulePublishedHandler
    {
        private readonly IRuleProvider _provider;
        private readonly IPipelineCache _cache;
        private readonly PricingPipelineCompiler _compiler;
        private readonly ILogger<RulePublishedHandler> _logger;

        public RulePublishedHandler(
            IRuleProvider provider,
            IPipelineCache cache,
            PricingPipelineCompiler compiler,
            ILogger<RulePublishedHandler> logger)
        {
            _provider = provider;
            _cache = cache;
            _compiler = compiler;
            _logger = logger;
        }

        public async Task HandleAsync(CancellationToken ct)
        {
            _logger.LogInformation("Reloading pricing pipeline from RuleService");

            var rules = await _provider.GetRulesAsync(ct);

            var pipeline = _compiler.Compile(rules);

            if(rules.Count > 0)
                _cache.Set(pipeline);

            _logger.LogInformation("Pricing pipeline reloaded with {RuleCount} rules", rules.Count);
        }
    }
}
