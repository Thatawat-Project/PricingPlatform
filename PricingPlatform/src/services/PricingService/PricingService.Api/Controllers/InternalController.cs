using Microsoft.AspNetCore.Mvc;
using PricingService.Application.Handlers;

namespace PricingService.Api.Controllers
{
    [ApiController]
    [Route("internal")]
    public sealed class InternalController : ControllerBase
    {
        private readonly RulePublishedHandler _handler;
        private readonly ILogger<InternalController> _logger;

        public InternalController(RulePublishedHandler handler, ILogger<InternalController> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        [HttpPost("rules/reload")]
        public async Task<IActionResult> Reload(CancellationToken ct)
        {
            _logger.LogInformation("Rule reload triggered via webhook");
            await _handler.HandleAsync(ct);
            return Ok();
        }
    }
}
