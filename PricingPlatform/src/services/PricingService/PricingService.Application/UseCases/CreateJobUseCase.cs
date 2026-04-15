using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.SharedKernel.Result;
using PricingService.Application.DTOs;
using PricingService.Application.Interfaces;
using PricingService.Domain.Entities;

namespace PricingService.Application.UseCases
{
    public sealed class CreateJobUseCase : ICreateJobUseCase
    {
        private readonly IJobQueue _queue;
        private readonly IJobCache _jobCache;

        public CreateJobUseCase(
            IJobQueue queue,
            IJobCache jobCache)
        {
            _queue = queue;
            _jobCache = jobCache;
        }

        public async Task<Result<CreateJobResult>> ExecuteAsync(IReadOnlyList<QuoteRequest> requests,CancellationToken ct)
        {
            if (requests == null || requests.Count == 0)
                return Result<CreateJobResult>.Failure("Requests cannot be empty");

            try
            {
                var jobId = Guid.NewGuid().ToString("N");

                var job = new PricingJob(jobId, requests);

                _jobCache.Add(job);

                await _queue.EnqueueAsync(job, ct);

                return Result<CreateJobResult>.Success(new CreateJobResult
                {
                    JobId = jobId
                });
            }
            catch (Exception ex)
            {
                return Result<CreateJobResult>.Failure(ex.Message);
            }
        }
    }
}
