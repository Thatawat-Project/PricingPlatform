using Microsoft.AspNetCore.Mvc;
using PricingService.Application.Interfaces;
using PricingService.Application.UseCases;

namespace PricingService.Api.Controllers
{
    [ApiController]
    [Route("jobs")]
    public sealed class JobsController : ControllerBase
    {
        private readonly IGetJobUseCase _getJobUseCase;

        public JobsController(IGetJobUseCase getJobUseCase)
        {
            _getJobUseCase = getJobUseCase;
        }

        [HttpGet("{jobId}")]
        public IActionResult Get(string jobId)
        {
            var result = _getJobUseCase.Execute(jobId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}