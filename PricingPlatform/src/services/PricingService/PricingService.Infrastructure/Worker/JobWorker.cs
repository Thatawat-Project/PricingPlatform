using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.Engine.Core;
using PricingService.Application.Interfaces;
using PricingService.Domain.Entities;

namespace PricingService.Infrastructure.Worker
{
    public sealed class JobWorkerPool : BackgroundService
    {
        private readonly IJobQueue _queue;
        private readonly IJobCache _jobCache;
        private readonly IPipelineCache _pipelineCache;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<JobWorkerPool> _logger;
        private readonly int _workerCount;

        public JobWorkerPool(
            IJobQueue queue,
            IJobCache jobCache,
            IPipelineCache pipelineCache,
            IServiceScopeFactory scopeFactory,
            IConfiguration config,
            ILogger<JobWorkerPool> logger)
        {
            _queue = queue;
            _jobCache = jobCache;
            _pipelineCache = pipelineCache;
            _scopeFactory = scopeFactory;
            _logger = logger;

            _workerCount = config.GetValue<int?>("Worker:Count")
                            ?? Environment.ProcessorCount;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workers = new Task[_workerCount];

            for (int i = 0; i < _workerCount; i++)
            {
                workers[i] = WorkerLoop(i, stoppingToken);
            }

            return Task.WhenAll(workers);
        }

        private async Task WorkerLoop(int workerId, CancellationToken ct)
        {
            await foreach (var job in _queue.DequeueAllAsync(ct))
            {
                try
                {
                    await ProcessJobAsync(job, workerId, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Worker {WorkerId} failed processing job {JobId}",
                        workerId, job.Id);

                    job.Fail();
                    _jobCache.Update(job);
                }
            }
        }

        private Task ProcessJobAsync(PricingJob job, int workerId, CancellationToken ct)
        {
            job.Start();
            _jobCache.Update(job);

            if (!_pipelineCache.TryGet(out var pipeline))
            {
                job.Fail("Pipeline not ready");
                _jobCache.Update(job);
                return Task.CompletedTask;
            }

            var requests = job.Requests;
            var results = new QuoteResult[requests.Count];

            var now = DateTime.UtcNow;
            var hour = now.Hour;
            var day = now.DayOfWeek;

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = ct
            };

            Parallel.For(0, requests.Count, options, i =>
            {
                var req = requests[i];

                // stack-based struct (reduce GC pressure)
                var ctx = new PriceContext
                {
                    BasePrice = req.BasePrice,
                    Weight = req.Weight,
                    Zone = req.Zone,
                    HourOfDay = hour,
                    DayOfWeek = day
                };

                var price = pipeline.Execute(in ctx);

                results[i] = new QuoteResult
                {
                    Price = price
                };
            });

            job.Complete(results.ToList());
            _jobCache.Update(job);

            return Task.CompletedTask;
        }
    }
}
