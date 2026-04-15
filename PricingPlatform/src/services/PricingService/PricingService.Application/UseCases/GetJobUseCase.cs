using System;
using System.Collections.Generic;
using System.Text;
using PricingService.Application.DTOs;
using PricingService.Application.Interfaces;

namespace PricingService.Application.UseCases
{
    public sealed class GetJobUseCase : IGetJobUseCase
    {
        private readonly IJobCache _jobCache;

        public GetJobUseCase(IJobCache jobCache)
        {
            _jobCache = jobCache;
        }

        public JobResult? Execute(string jobId)
        {
            if (!_jobCache.TryGet(jobId, out var job))
                return null;

            return new JobResult
            {
                JobId = job.Id,
                Status = job.Status,
                TotalRequests = job.Requests.Count,
                CompletedRequests = job.Results?.Count,
                Results = job.Results,
                Error = job.Error,
                CreatedAtUtc = job.CreatedAtUtc,
                CompletedAtUtc = job.CompletedAtUtc
            };
        }
    }
}
