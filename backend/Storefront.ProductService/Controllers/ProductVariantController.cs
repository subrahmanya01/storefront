using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Services;

namespace Storefront.ProductService.Controllers
{
    [ApiController]
    [Route("Products/{productId}/Variants")]
    public class ProductVariantController : ControllerBase
    {
        private readonly ILogger<ProductVariantController> _logger;
        private readonly IProductService _productService;
        private readonly IValidator<ProductVariant> _validator;
        public ProductVariantController(ILogger<ProductVariantController> logger, IProductService service, IValidator<ProductVariant> validator)
        {
            _logger = logger;
            _productService = service;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string productId)
        {
            var variants = await _productService.GetVariantsAsync(productId);
            return Ok(variants);
        }

        [HttpGet("{variantId}")]
        public async Task<IActionResult> Get(string productId, string variantId)
        {
            var variant = await _productService.GetVariantByIdAsync(productId, variantId);
            return variant == null ? NotFound() : Ok(variant);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string productId, ProductVariant variant)
        {
            var context = new ValidationContext<ProductVariant>(variant);
            context.RootContextData["Mode"] = "create";

            var result = await _validator.ValidateAsync(context);
            if (!result.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetByIdAsync(productId);
            if (existingProduct == null) {
                return NotFound("Product not found");
            }
            if(variant.Attributes.Any(x => !existingProduct!.AllowedAttributes.Contains(x.Key)))
            {
                return BadRequest("Invalid attribute or attribute is not registered with product");
            }
            await _productService.AddVariantAsync(productId, variant);

            _logger.LogInformation("New varient created with for product {productId}", productId);
            return Ok(variant);
        }

        [HttpPut("{variantId}")]
        public async Task<IActionResult> Update(string productId, string variantId, [FromBody] ProductVariant variant)
        {
            var context = new ValidationContext<ProductVariant>(variant);
            context.RootContextData["Mode"] = "update";

            var result = await _validator.ValidateAsync(context);
            if (!result.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }
            if (variant.Attributes.Any(x => !existingProduct!.AllowedAttributes.Contains(x.Key)))
            {
                return BadRequest("Invalid attribute or attribute is not registered with product");
            }
            variant.Id = variant.Id;
            await _productService.UpdateVariantAsync(productId, variant.Id!, variant);
            _logger.LogInformation("Varient with id {varientId} updated for product {productId}", variant.Id, productId);
            return NoContent();
        }

        [HttpDelete("{variantId}")]
        public async Task<IActionResult> Delete(string productId, string variantId)
        {
            await _productService.DeleteVariantAsync(productId, variantId);
            return NoContent();
        }
    }
}
