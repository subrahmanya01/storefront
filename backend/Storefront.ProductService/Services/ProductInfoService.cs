using AutoMapper;
using MongoDB.Driver;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Infrastructure.Repository;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Models.Response;
using Storefront.ProductService.Services.GrpcClient;

namespace Storefront.ProductService.Services
{
    public class ProductInfoService : IProductInfoService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFlashSaleRepository _flashSaleRepository;
        private readonly string _azureBlobConnectionString;
        private readonly string _bannerImagesContainerName;
        private readonly IMapper _mapper;
        private readonly IProductRatingService _ratingService;
        private readonly IOrderRpcService _orderRpcService;

        public ProductInfoService(IProductRepository productRepository, IFlashSaleRepository flashSaleRepository, IConfiguration configuration, IMapper mapper, IProductRatingService ratingService, IOrderRpcService orderRpcService)
        {
            _productRepository = productRepository;
            _flashSaleRepository = flashSaleRepository;
            _azureBlobConnectionString = configuration["AzureBlob:ConnectionString"]!;
            _bannerImagesContainerName = configuration["AzureBlob:BannerImagesContainerName"]!;
            _mapper = mapper;
            _ratingService = ratingService;
            _orderRpcService = orderRpcService;
        }

        public async Task<List<string>> GetProductCategories()
        {
            var allProducts = await _productRepository.GetAsync(_ => true);
            var distinctCategories = allProducts
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category)
                .Distinct()
                .ToList();

            return distinctCategories;
        }

        private async Task AddProductRating(List<ProductAddResponse> responses)
        {
            var productIds = responses.Select(x => x.Id).ToList();
            var rating = await _ratingService.GetRatingsByProductIdsAsync(productIds!);
            Dictionary<string, int> map = new Dictionary<string, int>();
            foreach(var item in rating)
            {
                map[item.ProductId??""] = item.AvgRating;
            }

            foreach(var item in responses)
            {
                item.Rating = map!.GetValueOrDefault(item.Id);
            }
        }

        private async Task AddDiscounts(List<ProductAddResponse> responses)
        {
            try
            {
                var discounts = await _orderRpcService.GetAllDiscounts();
                foreach (var product in responses)
                {
                    product.Discounts = discounts.Discounts.Where(
                        x => (x.Category.ToLower() == product.Category.ToLower() || x.ProductId == product.Id) &&
                        string.IsNullOrWhiteSpace(x.Code) &&
                        DateTime.Parse(x.ValidTo) >= DateTime.UtcNow &&
                        DateTime.Parse(x.ValidFrom) <= DateTime.UtcNow
                        ).ToList();
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
           
        }

        private async Task AddShippingCharge(List<ProductAddResponse> responses)
        {
            try
            {
                var chargss = await _orderRpcService.GetAllShippingCharges();
                foreach (var product in responses)
                {
                    product.ShippingCharges = chargss.ShippingCharges.Where(x =>
                         (string.IsNullOrEmpty(x.EffectiveTo) ||
                         DateTime.Parse(x.EffectiveTo) >= DateTime.UtcNow) &&
                        (string.IsNullOrEmpty(x.EffectiveFrom) || DateTime.Parse(x.EffectiveFrom) <= DateTime.UtcNow) &&
                        x.ProductId == product.Id
                        ).ToList();
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
           
        }

        public async Task<List<ProductAddResponse>> GetProductsByCategoryAsync(string category)
        {
            var products =  await _productRepository.GetAsync(p => p.Category == category);
            var mappedProducts = _mapper.Map<List<ProductAddResponse>>(products);
            await AddProductRating(mappedProducts);
            await AddDiscounts(mappedProducts);
            await AddShippingCharge(mappedProducts);
            return mappedProducts;
        }

        public async Task<List<ProductAddResponse>> GetRecentProductsAsync()
        {
            var allProducts = await _productRepository.GetAsync(p => p.CreatedAt.HasValue);
            var products = allProducts
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToList();
            var mappedProducts = _mapper.Map<List<ProductAddResponse>>(products);
            await AddProductRating(mappedProducts);
            await AddDiscounts(mappedProducts);
            await AddShippingCharge(mappedProducts);
            return mappedProducts;
        }

        public async Task<List<FlashSale>> GetFlashSalesByProductAsync(string productId)
        {
            return await _flashSaleRepository.GetAsync(f => f.ProductId == productId);
        }

        public async Task<List<ProductAddResponse>> GetActiveFlashSalesAsync()
        {
            var now = DateTime.UtcNow;
            var flashSales = await _flashSaleRepository.GetAsync(f => f.StartsAt <= now && f.EndsAt >= now);
            var productIds = flashSales.Select(x=> x.ProductId).ToList();
            var filter = Builders<Product>.Filter.In(p => p.Id, productIds);
            var products = await _productRepository.GetAsync(filter);
            var mappedProducts = _mapper.Map<List<ProductAddResponse>>(products);
            await AddProductRating(mappedProducts);
            await AddDiscounts(mappedProducts);
            await AddShippingCharge(mappedProducts);
            return mappedProducts;
        }

        public async Task<FlashSale> AddFlashSaleAsync(FlashSale flashSale)
        {
            if (flashSale.StartsAt >= flashSale.EndsAt)
            {
                throw new InvalidOperationException("End date must be later than start date.");
            }

            return await _flashSaleRepository.InsertAsync(flashSale);
        }

        public async Task<List<FlashSale>> AddFlashRangeSaleAsync(List<FlashSaleRequestModel> flashSales)
        {
            var productIds = flashSales.Select(x => x.ProductId).ToList();
            var filter = Builders<Product>.Filter.In(p => p.Id, productIds);
            var products = await _productRepository.GetAsync(filter);
            if(products.Count != productIds.Count)
            {
                return null!;
            }
            var mappedRequest = _mapper.Map<List<FlashSale>>(flashSales);
            return await _flashSaleRepository.InsertRangeAsync(mappedRequest);
        }

        public async Task<bool> DeleteFlashSaleAsync(string flashSaleId)
        {
            return await _flashSaleRepository.DeleteAsync(flashSaleId);
        }

        public async Task<List<FlashSale>> GetActiveFlashSalesInfoAsync()
        {
            var now = DateTime.UtcNow;
            var flashSales = await _flashSaleRepository.GetAsync(f => f.StartsAt <= now && f.EndsAt >= now);
            return flashSales;
        }

        public async Task<List<FlashSale>> GetAllFlashSalesInfoAsync()
        {
            var flashSales = await _flashSaleRepository.GetAsync(f =>true);
            return flashSales;
        }

        public async Task<List<ProductAddResponse>> GetProductsByIds(GetMultipleProductsByIdsRequest request)
        {
            var products = await _productRepository.GetAsync(_ => request.ProductIds!.Contains(_.Id!));
            var mappedProducts = _mapper.Map<List<ProductAddResponse>>(products);
            await AddProductRating(mappedProducts);
            await AddDiscounts(mappedProducts);
            await AddShippingCharge(mappedProducts);
            return mappedProducts;
        }

        public async Task<List<string>> GetBrands()
        {
            var allProducts = await _productRepository.GetAsync(_ => true);
            var brands = allProducts
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.BaseAttributes.GetValueOrDefault("Brand"))
                .Distinct()
                .ToList();

            return brands?.Distinct()?.ToList()! ?? new List<string>();
        }
    }
}
