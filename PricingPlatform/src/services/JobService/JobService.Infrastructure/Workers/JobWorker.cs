using System;
using System.Collections.Generic;
using System.Text;
using JobService.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using PricingPlatform.Contracts.DTOs;

namespace JobService.Infrastructure.Workers
{
    /// <summary>
    /// Background worker that consumes job queue and executes pricing calculation
    /// </summary>
    public sealed class JobWorker : BackgroundService
    {
        private readonly IJobQueue _queue;
        private readonly IJobRepository _repository;
        private readonly IPricingEngine _pricingEngine;

        public JobWorker(
            IJobQueue queue,
            IJobRepository repository,
            IPricingEngine pricingEngine)
        {
            _queue = queue;
            _repository = repository;
            _pricingEngine = pricingEngine;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var job in _queue.DequeueAllAsync(stoppingToken))
            {
                try
                {
                    job.Start();

                    var results = new List<QuoteResult>(job.Requests.Count);

                    foreach (var request in job.Requests)
                    {
                        var price = _pricingEngine.Calculate(request);
                        results.Add(new QuoteResult(price));
                    }

                    job.Complete(results);

                    _repository.Update(job);
                }
                catch
                {
                    job.Fail();
                    _repository.Update(job);
                }
            }
        }
    }
}
