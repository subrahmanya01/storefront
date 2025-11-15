using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Services;

namespace Storefront.OrderAndShippingService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor contextAccessor)
        {
            _orderService = orderService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);

            var order = await _orderService.CreateOrderAsync(userId, request);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return order is not null ? Ok(order) : NotFound();
        }

        [HttpGet("Customer")]
        public async Task<IActionResult> GetOrdersByCustomerId()
        {
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);
            var orders = await _orderService.GetOrdersByCustomerIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderService.GetAllOrders());
        }

        [HttpDelete("Cancel/{id}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var result = await _orderService.CancelOrder(id);
            if(result)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("Status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var result = await _orderService.UpdateOrderStatus(request);
            if(result == null)
            {
                return NotFound("Order with given order id does not exist");
            }
            return Ok(result);
        }
    }
}
