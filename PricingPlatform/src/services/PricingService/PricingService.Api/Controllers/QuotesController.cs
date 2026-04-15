using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PricingPlatform.Contracts.DTOs;
using PricingService.Application.DTOs;
using PricingService.Application.Interfaces;
using PricingService.Application.UseCases;
using PricingService.Infrastructure.Parsers;

namespace PricingService.Api.Controllers
{
    [ApiController]
    [Route("quotes")]
    public sealed class QuotesController : ControllerBase
    {
        private readonly IPriceQuoteUseCase _priceUseCase;
        private readonly ICreateJobUseCase _createJobUseCase;

        public QuotesController(IPriceQuoteUseCase priceUseCase, ICreateJobUseCase createJobUseCase)
        {
            _priceUseCase = priceUseCase;
            _createJobUseCase = createJobUseCase;
        }

        [HttpPost("price")]
        public IActionResult Price(QuoteRequest ctx)
        {
            var result = _priceUseCase.Price(ctx);

            if (!result.IsSuccess)
                return Problem(detail: result.Error, statusCode: 503);

            return Ok(new
            {
                result.Value!.Price,
                result.IsFallback,
                result.Error
            });
        }

        [EnableRateLimiting("bulk-policy")]
        [HttpPost("bulk")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> Bulk(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            IReadOnlyList<QuoteRequest> requests;

            await using (var stream = file.OpenReadStream())
            {
                requests = await BulkFileParser.ParseAsync(file.FileName, stream, ct);
            }

            var result = await _createJobUseCase.ExecuteAsync(requests, ct);

            if (!result.IsSuccess)
                return BadRequest(new
                {
                    error = result.Error
                });

            return Accepted(new
            {
                result.Value!.JobId,
                result.IsFallback,
                result.Error
            });
        }
    }
}
