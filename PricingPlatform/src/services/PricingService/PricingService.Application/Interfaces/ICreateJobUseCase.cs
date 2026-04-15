using System;
using System.Collections.Generic;
using System.Text;
using PricingPlatform.Contracts.DTOs;
using PricingPlatform.SharedKernel.Result;
using PricingService.Application.DTOs;

namespace PricingService.Application.Interfaces
{
    public interface ICreateJobUseCase
    {
        Task<Result<CreateJobResult>> ExecuteAsync(IReadOnlyList<QuoteRequest> requests, CancellationToken ct);
    }
}
