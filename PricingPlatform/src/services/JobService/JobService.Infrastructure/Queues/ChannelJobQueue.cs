using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using JobService.Application.Interfaces;
using JobService.Domain.Entities;

namespace JobService.Infrastructure.Queues
{
    /// <summary>
    /// In-memory bounded queue using Channel (thread-safe, backpressure supported)
    /// </summary>
    public sealed class ChannelJobQueue : IJobQueue
    {
        private readonly Channel<PricingJob> _channel;

        public ChannelJobQueue(int capacity = 1000)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            };

            _channel = Channel.CreateBounded<PricingJob>(options);
        }

        public ValueTask EnqueueAsync(PricingJob job, CancellationToken ct)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));

            return _channel.Writer.WriteAsync(job, ct);
        }

        public IAsyncEnumerable<PricingJob> DequeueAllAsync(CancellationToken ct)
        {
            return _channel.Reader.ReadAllAsync(ct);
        }
    }
}
