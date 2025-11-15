using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Services;

namespace Storefront.OrderAndShippingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShippingChargeController : ControllerBase
    {
        private readonly IShippingChargeService _service;

        public ShippingChargeController(IShippingChargeService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ShippingChargeRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(Guid id, [FromBody] ShippingChargeRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("GetPostalCodes")]
        public async Task<IActionResult> GetPostalCodes()
        {
            var result = await _service.GetPostalCodes();
            if (result == null)
            {
                return NotFound("No Postal codes added yet");
            }
            return Ok(result.Where(x=> !string.IsNullOrWhiteSpace(x)));
        }
    }

}
