using System;
using System.Collections.Generic;
using System.Text;

namespace PricingPlatform.Engine.Core
{
    public readonly struct PipelineStep
    {
        public readonly Rule Rule;

        public PipelineStep(in Rule rule)
        {
            Rule = rule;
        }
    }
}
