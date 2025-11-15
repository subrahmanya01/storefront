using Microsoft.AspNetCore.Mvc;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Services;

namespace Storefront.OrderAndShippingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderPriceController : ControllerBase
    {
        private readonly IOrderPriceService _orderPriceService;
        public OrderPriceController(IOrderPriceService orderPriceService)
        {
            _orderPriceService = orderPriceService; 
        }

        [HttpPost("GetOrderAmount")]
        public async Task<IActionResult> GetOrderPrice([FromBody] OrderPriceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var result = await _orderPriceService.CalculateOrderPrice(request);
            if (result == null)
            {
                return NotFound("Order with given order id does not exist");
            }
            return Ok(result);
        }
    }
}
