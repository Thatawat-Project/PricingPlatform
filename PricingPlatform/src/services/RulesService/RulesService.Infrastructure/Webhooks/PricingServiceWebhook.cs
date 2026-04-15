using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using RulesService.Application.Interface;

namespace RulesService.Infrastructure.Webhooks
{
    public sealed class PricingServiceWebhook : IPricingServiceWebhook
    {
        private readonly HttpClient _http;
        private readonly ILogger<PricingServiceWebhook> _logger;

        public PricingServiceWebhook(HttpClient http, ILogger<PricingServiceWebhook> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<bool> NotifyRuleChangedAsync(CancellationToken ct)
        {
            try
            {
                using var req = new HttpRequestMessage(
                    HttpMethod.Post,
                    "/internal/rules/reload");

                req.Headers.Add("X-Event-Id", Guid.NewGuid().ToString("N"));

                var res = await _http.SendAsync(req, ct);

                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PricingService reload failed: {StatusCode}", res.StatusCode);
                    return false;
                }

                _logger.LogInformation("PricingService notified successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PricingService webhook failed");
                return false;
            }
        }

        public async Task<bool> PingAsync(CancellationToken ct)
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Get, "/health");

                using var response = await _http.SendAsync(req,HttpCompletionOption.ResponseHeadersRead,ct);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PricingService ping failed");
                return false;
            }
        }
    }
}
