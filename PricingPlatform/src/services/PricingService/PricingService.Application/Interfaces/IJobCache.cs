using System;
using System.Collections.Generic;
using System.Text;
using PricingService.Domain.Entities;

namespace PricingService.Application.Interfaces
{
    public interface IJobCache
    {
        void Add(PricingJob job);
        bool TryGet(string jobId, out PricingJob job);
        void Update(PricingJob job);
    }
}
