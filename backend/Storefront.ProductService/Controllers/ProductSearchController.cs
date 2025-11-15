using Microsoft.AspNetCore.Mvc;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Services;

namespace Storefront.ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductSearchController : ControllerBase
    {
        private readonly ILogger<ProductSearchController> _logger;   
        private readonly ISearchService _searchService;
        public ProductSearchController(ILogger<ProductSearchController> logger, ISearchService searchService)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpPost]
        [Route("SearchProduct")]
        public async Task<IActionResult> SearchProducts([FromBody] SearchRequest request)
        {
            if(request== null)
            {
                return BadRequest();
            }
            var result = await _searchService.GetSearchResult(request);
            _logger.LogInformation("Search results fetched successfully");
            return Ok(result);
        }
    }
}
