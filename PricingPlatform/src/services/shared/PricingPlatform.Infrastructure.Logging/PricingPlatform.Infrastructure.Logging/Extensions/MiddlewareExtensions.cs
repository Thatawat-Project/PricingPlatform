using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using PricingPlatform.Infrastructure.Logging.Middleware;

namespace PricingPlatform.Infrastructure.Logging.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseObservability(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseApiDocs(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
