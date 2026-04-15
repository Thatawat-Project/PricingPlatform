using System;
using System.Collections.Generic;
using System.Text;
using PricingService.Application.DTOs;

namespace PricingService.Application.Interfaces
{
    public interface IGetJobUseCase
    {
        JobResult? Execute(string jobId);
    }
}
