using System;
using System.Collections.Generic;
using System.Text;
using JobService.Application.Interfaces;
using JobService.Domain.Entities;

namespace JobService.Application.UseCases
{
    public sealed class GetJobUseCase
    {
        private readonly IJobRepository _repository;

        public GetJobUseCase(IJobRepository repository)
        {
            _repository = repository;
        }

        public PricingJob? Execute(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
                return null;

            return _repository.TryGet(jobId, out var job)
                ? job
                : null;
        }
    }
}
