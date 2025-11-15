using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storefront.CartService.Models.Request;
using Storefront.CartService.Services;

namespace Storefront.CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetCart()
        {
            var userId = _userService.GetUserId();
            var cart = await _cartService.GetCartItemsAsync(userId);
            return Ok(cart);
        }

        [HttpPost("Add-Item")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemRequest cartItemDto)
        {
            var result = await _cartService.AddItemToCartAsync(_userService.GetUserId(), cartItemDto);
            if(result.Status != StatusCodes.Status200OK)
            {
                return StatusCode(result.Status, result.Message);
            }
            return Ok(result.IsSuccess);
        }

        [HttpPut("Update-Quantity/{itemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(Guid itemId, [FromBody] UpdateQuantityRequest updateQuantityDto)
        {
            var result = await _cartService.UpdateCartItemQuantityAsync(itemId, updateQuantityDto.Quantity);
            return Ok(result);
        }

        [HttpDelete("Remove-Item/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(Guid itemId)
        {
            await _cartService.RemoveItemFromCartAsync(itemId);
            return NoContent();
        }

        [HttpDelete("Clear")]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync(_userService.GetUserId());
            return NoContent();
        }
    }
}
