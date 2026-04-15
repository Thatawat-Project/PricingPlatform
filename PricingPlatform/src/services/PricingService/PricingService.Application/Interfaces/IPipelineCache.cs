using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Engine.Core;

namespace PricingService.Application.Interfaces
{
    public interface IPipelineCache
    {
        bool TryGet(out CompiledPricingPipeline pipeline);
        void Set(CompiledPricingPipeline pipeline);
    }
}
