using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Services;

namespace Storefront.ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IValidator<Product> _validator;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IValidator<Product> validator)
        {
            _logger = logger;
            _productService = productService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            _logger.LogInformation("Products fetched successfully");
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPaginatedProducts(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productService.GetPaginatedProductsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            var context = new ValidationContext<Product>(product);
            context.RootContextData["Mode"] = "create";

            var result = await _validator.ValidateAsync(context);
            if (!result.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _productService.IsAnyProductWithSameNameExist(product.Name))
            {
                return Conflict("Product with same name already exist");
            }
            await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] Product product)
        {
            var context = new ValidationContext<Product>(product);
            context.RootContextData["Mode"] = "update";

            var result = await _validator.ValidateAsync(context);
            if (!result.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _productService.IsAnyProductWithSameNameExist(product.Name, product.Id))
            {
                return Conflict("Product with same name already exist");
            }

            var existing = await _productService.GetByIdAsync(product.Id);
            if (existing == null) return NotFound();

            product.Id = existing.Id;
            product.CreatedAt = existing.CreatedAt;
            if(product.Variants == null)
            {
                product.Variants = existing.Variants;
            }
            await _productService.UpdateAsync(product.Id!, product);
            _logger.LogInformation("Product with id {id} updated successfully", product.Id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _productService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _productService.DeleteAsync(id);
            _logger.LogInformation("Product with id {id} deleted successfully", id);
            return NoContent();
        }
    }
}
