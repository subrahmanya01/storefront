using Microsoft.AspNetCore.Mvc;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Services;
using Microsoft.AspNetCore.Authorization;

namespace Storefront.OrderAndShippingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateDiscount([FromBody] DiscountRequest discount)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var created = await _discountService.CreateDiscountAsync(discount);
            return CreatedAtAction(nameof(GetDiscountById), new { id = created.Id }, created);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateDiscount(Guid id, [FromBody] DiscountRequest discount)
        {
            var updated = await _discountService.UpdateDiscountAsync(id, discount);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllDiscounts()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return Ok(discounts);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetDiscountById(Guid id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null)
                return NotFound();
            return Ok(discount);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchDiscounts(
            [FromQuery] string? code,
            [FromQuery] string? category,
            [FromQuery] string? productId)
        {
            var results = await _discountService.SearchDiscountsAsync(code, category, productId);
            return Ok(results);
        }

        [HttpPost("Validate")]

        public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponRequest request)
        {
            var result = await _discountService.ValidateCouponAsync(request);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEntry(Guid id)
        {
            var result = await _discountService.DeleteEntry(id);
            return Ok(result);
        }

    }

}
