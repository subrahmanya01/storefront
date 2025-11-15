using Microsoft.AspNetCore.Mvc;
using Storefront.ProductService.Extensions;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Services;

namespace Storefront.ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductInfoController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductInfoService _productInfoService;
        private readonly IBlobService _blobService;

        public ProductInfoController(ILogger<ProductInfoController> logger, IProductInfoService productInfoService, IBlobService blobService)
        {
            _logger = logger;
            _productInfoService = productInfoService;
            _blobService = blobService;
        }


        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> GetProductCategories()
        {
            return Ok(await _productInfoService.GetProductCategories());
        }

        [HttpGet]
        [Route("Categories/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var result = await _productInfoService.GetProductsByCategoryAsync(category);
            result.Shuffle();
            return Ok(result);
        }

        [HttpPost]
        [Route("Products/Multiple/Get")]
        public async Task<IActionResult> GetMultipleProductsByIds([FromBody] GetMultipleProductsByIdsRequest request)
        {
            if(request == null || request.ProductIds?.Count() == 0)
            {
                return BadRequest("Invalid Request");
            }
            var result = await _productInfoService.GetProductsByIds(request);
            result.Shuffle();
            return Ok(result);
        }


        [HttpGet]
        [Route("FlashSales/Products")]
        public async Task<IActionResult> GetFlashSaleProducts()
        {
            return Ok(await _productInfoService.GetActiveFlashSalesAsync());
        }

        [HttpGet]
        [Route("FlashSales/All")]
        public async Task<IActionResult> GetAllFlashSales()
        {
            return Ok(await _productInfoService.GetAllFlashSalesInfoAsync());
        }

        [HttpGet]
        [Route("FlashSales")]
        public async Task<IActionResult> GetFlashSales()
        {
            return Ok(await _productInfoService.GetActiveFlashSalesAsync());

        }

        [HttpPost]
        [Route("FlashSales")]
        public async Task<IActionResult> AddFlashSaleProducts([FromBody] List<FlashSaleRequestModel> request)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var result = await _productInfoService.AddFlashRangeSaleAsync(request);
            if(result == null)
            {
                return NotFound("Some products are not found");
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("FlashSales/{id}")]
        public async Task<IActionResult> DeleteFlashSaleProducts(string id)
        {
            return Ok(await _productInfoService.DeleteFlashSaleAsync(id));
        }

        [HttpGet]
        [Route("Banner")]
        public async Task<IActionResult> GetProductBanner()
        {
            var result = await _blobService.GetAllAsync("banners/");
            return Ok(result);
        }

        [HttpPost]
        [Route("Banner")]
        public async Task<IActionResult> AddProductBanners([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }

            var uploadedUrls = new List<string>();
            foreach (var file in files)
            {
                var fileName = $"banners/{Guid.NewGuid()}_{file.FileName}";
                var imageUrl = await _blobService.UploadAsync(file, fileName);
                uploadedUrls.Add(imageUrl);
            }
            return Ok(uploadedUrls);
        }

        [HttpDelete]
        [Route("Banner")]
        public async Task<IActionResult> DeleteProductBanner([FromQuery] string imageUrl)
        {
            await _blobService.DeleteAsync(GetFileNameFromUrl(imageUrl));
            return NoContent();
        }


        [HttpGet]
        [Route("Brands")]
        public async Task<IActionResult> GetBrands()
        {
            var response = await _productInfoService.GetBrands();
            return Ok(response);
        }

        private string GetFileNameFromUrl(string url)
        {
            var index = url.IndexOf("banners/");
            return index > -1 ? url.Substring(index) : Path.GetFileName(url);
        }
    }
}
