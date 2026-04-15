using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using PricingService.Application.Interfaces;
using PricingService.Domain.Entities;

namespace PricingService.Infrastructure.Queue
{
    public sealed class ChannelJobQueue : IJobQueue
    {
        private readonly Channel<PricingJob> _channel;

        public ChannelJobQueue()
        {
            var options = new BoundedChannelOptions(capacity: 1000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            };

            _channel = Channel.CreateBounded<PricingJob>(options);
        }

        public ValueTask EnqueueAsync(PricingJob job, CancellationToken ct)
        {
            if (job is null)
                throw new ArgumentNullException(nameof(job));

            return _channel.Writer.WriteAsync(job, ct);
        }

        public IAsyncEnumerable<PricingJob> DequeueAllAsync(CancellationToken ct)
        {
            return _channel.Reader.ReadAllAsync(ct);
        }
    }
}
