using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace PricingPlatform.Infrastructure.Logging.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();

            context.Items["CorrelationId"] = correlationId;
            context.Response.Headers[CorrelationIdHeader] = correlationId;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
            using (Serilog.Context.LogContext.PushProperty("RequestPath", context.Request.Path))
            {
                await _next(context);
            }
        }
    }
}
