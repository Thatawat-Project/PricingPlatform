using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Engine.Core;
using PricingService.Application.Interfaces;

namespace PricingService.Infrastructure.Cache
{
    public sealed class InMemoryPipelineCache : IPipelineCache
    {
        private const string Key = "active-pipeline";

        private readonly ConcurrentDictionary<string, CompiledPricingPipeline> _cache = new();

        public bool TryGet(out CompiledPricingPipeline pipeline)
            => _cache.TryGetValue(Key, out pipeline!);

        public void Set(CompiledPricingPipeline pipeline)
            => _cache[Key] = pipeline;
    }
}
