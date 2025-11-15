using Microsoft.AspNetCore.Mvc;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Services;

namespace Storefront.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WhishListController : ControllerBase
    {
        private readonly IWhishListService _whishListService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WhishListController(IWhishListService whishListService, IHttpContextAccessor httpContextAccessor)
        {
            _whishListService = whishListService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {

            var userInfo = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);
            var items = await _whishListService.GetItemsAsync(userId);
            return Ok(items);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] AddWhishListItemRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.ProductId))
            {
                return BadRequest("Invalid product Id");
            }

            var userInfo = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);
            await _whishListService.AddItemAsync(userId, request.ProductId!);
            return NoContent();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveItem([FromBody] RemoveWhishListItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ProductId))
            {
                return BadRequest("Invalid product Id");
            }

            var userInfo = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim); 
            await _whishListService.RemoveItemAsync(userId, request.ProductId!);
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {

            var userInfo = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }
            var userId = Guid.Parse(userIdClaim);
            await _whishListService.ClearAsync(userId);
            return NoContent();
        }
    }
}
