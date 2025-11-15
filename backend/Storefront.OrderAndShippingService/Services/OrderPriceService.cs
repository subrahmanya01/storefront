using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Infrastructure;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;
using Storefront.OrderAndShippingService.Protos;
using Storefront.OrderAndShippingService.Services.GrpcClient;

namespace Storefront.OrderAndShippingService.Services
{
    public class OrderPriceService : IOrderPriceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRpcService _productRpcService;
        public OrderPriceService(ApplicationDbContext applicationDbContext, IProductRpcService productRpcService)
        {
            _context = applicationDbContext;
            _productRpcService = productRpcService;
        }

        public async Task<OrderPriceResponse> CalculateOrderPrice(OrderPriceRequest request)
        {
            var productIds = request.ProductInfomations.Select(x => x.ProductId).ToList();
            var productsInfo = await _productRpcService.GetProductsById(productIds!);
            // calculate discounts for the products
            var discounts = GetDiscounts(productsInfo, request);
            // Add tax for the selected products
            var tax = await GetTaxAmount(productsInfo, request);
            // Add shipping charge for the selected postal code
            var shippingFee = await GetShippingCharge(productIds!, request);
            var response = new OrderPriceResponse()
            {
                ShippingFee = shippingFee,
                Discounts = discounts,
                Tax = tax,
            };

            var totalAmount = await GetTotalAmount(response);
            response.TotalPrice = totalAmount;
            return response;
        }


        public async Task<double> GetTotalAmount(OrderPriceResponse priceInfo)
        {
            double subTotal = 0;
            var productIds = priceInfo.Discounts!.Select(x => x.ProductId).ToList();
            var productsInfo = await _productRpcService.GetProductsById(productIds!);
            foreach (var product in productsInfo) {
                var discountInfo = priceInfo.Discounts!.FirstOrDefault(x => x.ProductId == product.Id);
                subTotal += product.Variants.FirstOrDefault(x => x.Id == discountInfo?.VariantId)!.Price - ((discountInfo!.Discount ?? 0) / 100) * product.Variants.FirstOrDefault(x => x.Id == discountInfo.VariantId)!.Price;
            }

            double total = subTotal + priceInfo.Tax + priceInfo.ShippingFee ?? 0;
            return total;
        }

        private async Task<double> GetShippingCharge(List<string> productIds, OrderPriceRequest request)
        {
            var shippingFee = GetBestShippingCharge(productIds, request.PostalCode!, request.SubTotal);

            return shippingFee;
        }

        private double GetBestShippingCharge(List<string> productIds, string postalCode, double subtotal)
        {
            var now = DateTime.UtcNow;

            var shippingEntries = _context.ShippingCharges
                .Where(s =>
                    (s.ProductId == null || productIds.Contains(s.ProductId)) &&
                    (string.IsNullOrEmpty(s.Region) || s.Region == postalCode) &&
                    (!s.EffectiveFrom.HasValue || s.EffectiveFrom <= now) &&
                    (!s.EffectiveTo.HasValue || s.EffectiveTo >= now) &&
                    s.MinOrderAmount <= (decimal)subtotal &&
                    (!s.MaxOrderAmount.HasValue || (decimal)subtotal <= s.MaxOrderAmount.Value)
                )
                .ToList();

            var productToBestShipping = new List<ShippingCharge>();

            foreach (var productId in productIds)
            {
                var candidates = shippingEntries
                    .Where(s => (s.ProductId == null || s.ProductId == productId) && (string.IsNullOrEmpty(s.Region) || s.Region == postalCode))
                    .ToList();

                if (!candidates.Any())
                    continue;

                // Choose best for product: prefer IsFree, then lowest ShippingFeePerKm
                var best = candidates
                    .OrderBy(s => s.IsFree ? 0 : 1) // prefer free
                    .ThenBy(s => s.ShippingFeePerKm) // prefer cheaper
                    .First();

                productToBestShipping.Add(best);
            }

            // If all matched best-fit entries are free, return 0
            if (productToBestShipping.All(s => s.IsFree))
                return 0;

            // Else return max shipping fee among non-free entries
            var maxFee = productToBestShipping
                .Where(s => !s.IsFree)
                .Max(s => s.ShippingFeePerKm);

            return (double)maxFee;
        }


        private async Task<double> GetTaxAmount(List<ProductResponse> products, OrderPriceRequest request)
        {
            var productCategories = products.Select(x => x.Category).ToList();
            var taxEntries = await _context.TaxRates.AsNoTracking().Where(x=> x.Category == null || productCategories.Contains(x.Category)).ToListAsync();
            double taxTotal = 0;

            foreach (var product in products) {
                var productRequest = request.ProductInfomations.Find(x => x.ProductId == product.Id);
                var productVariant = product.Variants.FirstOrDefault(x => x.Id == productRequest?.VariantId);

                var categoryTax = taxEntries.Find(x => x.Category == product.Category);
                if (categoryTax == null) {
                    var productPriceWithQuantity = (double)(productVariant!.Price * productRequest!.Quantity);
                    var taxPercentage = (double)taxEntries.FirstOrDefault(x => x.Category == null)!.Rate;

                    taxTotal += (taxPercentage/ 100) * productPriceWithQuantity;
                }
                else
                {
                    var productPriceWithQuantity = (double)(productVariant!.Price * productRequest!.Quantity);
                    var taxPercentage = (double)categoryTax.Rate;

                    taxTotal += (taxPercentage / 100) * productPriceWithQuantity;
                }
            }
            return taxTotal;
        }


        private List<ProductDiscount> GetDiscounts(List<ProductResponse> products, OrderPriceRequest request)
        {
            var discounts = new List<ProductDiscount>();
            var currentTime = DateTime.UtcNow;
            for(int i =0; i< products.Count; i++)
            {
                var discount = GetBestDiscountForProduct(products[i].Id, products[i].Category, request.SubTotal, request.CouponCode);
                if (discount != null)
                {
                    discounts.Add(new ProductDiscount { ProductId = request.ProductInfomations[i].ProductId, VariantId = request.ProductInfomations[i].VariantId, Discount = (double)discount.Percentage });
                }
            }
            return discounts;
        }
        private Discount? GetBestDiscountForProduct(string productId, string category, double subtotal, string? couponCode = null)
        {
            var now = DateTime.UtcNow;

            var discounts = _context.Discounts
                .Where(d => d.ValidFrom <= now && d.ValidTo >= now)
                .AsEnumerable();

            // Optional coupon filter
            if (!string.IsNullOrEmpty(couponCode))
            {
                discounts = discounts.Where(d => d.Code == couponCode);
            }
            else
            {
                discounts = discounts.Where(d => string.IsNullOrEmpty(d.Code));
            }

            // Filter applicable discounts
            var applicableDiscounts = discounts.Where(d =>
                (string.IsNullOrEmpty(d.ProductId) || d.ProductId == productId) &&
                (string.IsNullOrEmpty(d.Category) || d.Category == category) &&
                (!d.MinOrderAmount.HasValue || subtotal >= (double)d.MinOrderAmount.Value)
            );

            // Return the best (highest percentage) applicable discount
            return applicableDiscounts
                .OrderByDescending(d => d.Percentage)
                .ThenByDescending(d => !string.IsNullOrEmpty(d.ProductId)) // prioritize exact product match
                .ThenByDescending(d => !string.IsNullOrEmpty(d.Category))  // then category match
                .FirstOrDefault();
        }

    }
}
