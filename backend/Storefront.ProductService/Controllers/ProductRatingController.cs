using Microsoft.AspNetCore.Mvc;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Services;
using System.Security.Claims;

namespace Storefront.ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductRatingController : ControllerBase
    {
        private readonly IProductRatingService _ratingService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductRatingController(IProductRatingService ratingService, IHttpContextAccessor contextAccessor)
        {
            _ratingService = ratingService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetRatings(string productId)
        {
            var ratings = await _ratingService.GetRatingsAsync(productId);
            return Ok(ratings);
        }

        [HttpGet("UserRatings")]
        public async Task<IActionResult> GetUserRatingForProduct()
        {
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }

            if (userIdClaim == null) return Unauthorized();
            var ratings = await _ratingService.GetUserRatingForProducts(userIdClaim);
            return Ok(ratings);
        }

        [HttpPost("RatingByIds")]
        public async Task<IActionResult> GetRatings([FromBody] List<string> productIds)
        {
            var ratings = await _ratingService.GetRatingsByProductIdsAsync(productIds);
            return Ok(ratings);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] AddRatingRequest request)
        {
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("You are not authorized to do this operation");
            }

            if (userIdClaim == null) return Unauthorized();
            var userId = userIdClaim;

            var rating = await _ratingService.RateProductAsync(userId, request.ProductId, request.Rating, request.Comment);
            return Ok(rating);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(string id)
        {
            var success = await _ratingService.DeleteRatingAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
