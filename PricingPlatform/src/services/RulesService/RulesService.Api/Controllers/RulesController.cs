using Microsoft.AspNetCore.Mvc;
using RulesService.Application.DTOs;
using RulesService.Application.Interface;
using RulesService.Application.Services;

namespace RulesService.Api.Controllers
{
    [ApiController]
    [Route("/rules")]
    public class RulesController : ControllerBase
    {
        private readonly IRuleService _service;

        public RulesController(IRuleService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(CreateRuleRequest request)
        {
            var id = _service.Create(request);
            return Ok(new { id });
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
        {
            await _service.PublishAsync(id, ct);
            return Ok();
        }

        [HttpGet("active")]
        public IActionResult GetActive()
        {
            return Ok(_service.GetActive());
        }

        [HttpPut("{ruleId}")]
        public IActionResult UpdateCreateVersion(Guid ruleId, UpdateRuleRequest request)
        {
            var newId = _service.UpdateCreateVersion(ruleId, request);
            return Ok(new { id = newId });
        }
    }
}
