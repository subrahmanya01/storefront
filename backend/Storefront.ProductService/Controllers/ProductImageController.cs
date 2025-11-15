using Microsoft.AspNetCore.Mvc;
using Storefront.ProductService.Services;

namespace Storefront.ProductService.Controllers
{
    [ApiController]
    [Route("Products")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IBlobService _blobService;
        private readonly ILogger<ProductImageController> _logger;

        public ProductImageController(IProductService productService, IBlobService blobService, ILogger<ProductImageController> logger)
        {
            _productService = productService;
            _blobService = blobService;
            _logger = logger;
        }

        [HttpPost("{productId}/Image")]
        public async Task<IActionResult> UploadBaseProductImage(string productId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Only image files are allowed.");
            }

            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            var fileName = $"product-images/{productId}/{Guid.NewGuid()}_{file.FileName}";
            var imageUrl = await _blobService.UploadAsync(file, fileName);

            product.BaseAttributes.Add("image", imageUrl);
            await _productService.UpdateAsync(productId, product);

            _logger.LogInformation("Image uploaded for product {ProductId}", productId);
            return Ok(new { url = imageUrl });
        }

        [HttpGet("{productId}/Variants/{variantId}/Images")]
        public async Task<IActionResult> GetImages(string productId, string variantId)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            var variant = product.Variants?.FirstOrDefault(v => v.Id == variantId);
            if (variant == null)
            {
                return NotFound("Variant not found");
            }

            _logger.LogInformation("Fetched images for variant {VariantId} of product {ProductId}", variantId, productId);
            return Ok(variant.Images ?? new List<string>());
        }

        [HttpDelete("{productId}/Variants/{variantId}/Images")]
        public async Task<IActionResult> DeleteImages(string productId, string variantId, [FromQuery] List<string> imageUrls)
        {
            if (imageUrls == null || imageUrls.Count == 0)
            {
                return BadRequest("At least one imageUrl query parameter is required.");
            }

            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            var variant = product.Variants?.FirstOrDefault(v => v.Id == variantId);
            if (variant == null)
            {
                return NotFound("Variant not found");
            }

            if (variant.Images == null)
            {
                return BadRequest("Variant has no images.");
            }

            var notFoundImages = imageUrls.Where(url => !variant.Images.Contains(url)).ToList();
            if (notFoundImages.Any())
            {
                return BadRequest($"The following images were not found in the variant: {string.Join(", ", notFoundImages)}");
            }

            foreach (var imageUrl in imageUrls)
            {
                variant.Images.Remove(imageUrl);
                await _blobService.DeleteAsync(GetFileNameFromUrl(imageUrl));
            }

            await _productService.UpdateAsync(productId, product);

            _logger.LogInformation(
                "Deleted {Count} images from variant {VariantId} of product {ProductId}",
                imageUrls.Count, variantId, productId);

            return NoContent();
        }

        [HttpPost("{productId}/Variants/{variantId}/Images/Multiple")]
        public async Task<IActionResult> UploadImages(string productId, string variantId, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }

            var product = await _productService.GetByIdAsync(productId);
            if (product == null) return NotFound("Product not found");

            var variant = product.Variants?.FirstOrDefault(v => v.Id == variantId);
            if (variant == null) return NotFound("Variant not found");

            variant.Images ??= new List<string>();

            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                var fileName = $"product-images/{productId}/{variantId}/{Guid.NewGuid()}_{file.FileName}";
                var imageUrl = await _blobService.UploadAsync(file, fileName);
                variant.Images.Add(imageUrl);
                uploadedUrls.Add(imageUrl);
            }

            await _productService.UpdateAsync(productId, product);
            _logger.LogInformation("Uploaded {count} images to product {productId}, variant {variantId}", files.Count, productId, variantId);

            return Ok(uploadedUrls);
        }

        private string GetFileNameFromUrl(string url)
        {
            var index = url.IndexOf("product-images/");
            return index > -1 ? url.Substring(index) : Path.GetFileName(url);
        }
    }
}
