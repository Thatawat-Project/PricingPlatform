using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Engine.Core;
using PricingService.Application.Interfaces;

namespace PricingService.UnitTests.Fakes
{
    public sealed class FakePipelineCache : IPipelineCache
    {
        public CompiledPricingPipeline? Pipeline { get; set; }

        public bool TryGetLatest(out CompiledPricingPipeline? pipeline)
        {
            pipeline = Pipeline;
            return Pipeline is not null;
        }

        public bool TryGet(out CompiledPricingPipeline? pipeline)
        {
            pipeline = Pipeline;
            return Pipeline is not null;
        }

        public void Set(CompiledPricingPipeline pipeline)
        {
            Pipeline = pipeline;
        }
    }
}
