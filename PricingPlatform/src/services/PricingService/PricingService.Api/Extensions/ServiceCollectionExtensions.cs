using PricingPlatform.Engine.Core;
using PricingService.Application.Handlers;
using PricingService.Application.Interfaces;
using PricingService.Application.UseCases;
using PricingService.Infrastructure.Cache;
using PricingService.Infrastructure.Queue;
using PricingService.Infrastructure.RuleProvider;

namespace PricingService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpContextAccessor();
        return services;
    }

    public static IServiceCollection AddPricingServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<PricingPipelineCompiler>();
        services.AddSingleton<IPipelineCache, InMemoryPipelineCache>();
        services.AddSingleton<IJobQueue, ChannelJobQueue>();
        services.AddSingleton<IJobCache, InMemoryJobCache>();
        
        services.AddScoped<ICreateJobUseCase, CreateJobUseCase>();
        services.AddScoped<IGetJobUseCase, GetJobUseCase>();
        services.AddScoped<IPriceQuoteUseCase,PriceQuoteUseCase>();
        services.AddScoped<RulePublishedHandler>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<IRuleProvider, RuleServiceClient>(c =>
        {
            var baseUrl = config["RuleService:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
                c.BaseAddress = new Uri(baseUrl);
        });

        return services;
    }
}