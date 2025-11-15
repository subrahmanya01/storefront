using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Services;

namespace Storefront.OrderAndShippingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxRateController : ControllerBase
    {
        private readonly ITaxRateService _service;

        public TaxRateController(ITaxRateService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([FromBody] TaxRateRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("Get/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaxRateRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }

}
