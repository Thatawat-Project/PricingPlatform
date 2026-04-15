using System;
using System.Collections.Generic;
using System.Text;
using JobService.Domain.Entities;

namespace JobService.Application.Interfaces
{
    public interface IJobRepository
    {
        void Add(PricingJob job);

        bool TryGet(string jobId, out PricingJob job);

        void Update(PricingJob job);
    }
}
