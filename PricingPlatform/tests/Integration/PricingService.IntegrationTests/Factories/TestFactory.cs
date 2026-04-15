using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PricingService.Application.Interfaces;
using PricingService.IntegrationTests.Fake;

namespace PricingService.IntegrationTests.Factories
{
    public class TestFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // remove ทุก registration ของ IRuleProvider
                services.RemoveAll<IRuleProvider>();

                // inject fake
                services.AddSingleton<IRuleProvider>(new FakeRuleProvider());
            });
        }
    }
}
