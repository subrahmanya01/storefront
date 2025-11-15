using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public interface IDiscountService
    {
        Task<Discount> CreateDiscountAsync(DiscountRequest discount);
        Task<Discount?> UpdateDiscountAsync(Guid id, DiscountRequest discount);
        Task<List<Discount>> GetAllDiscountsAsync();
        Task<Discount?> GetDiscountByIdAsync(Guid id);
        Task<List<Discount>> SearchDiscountsAsync(string? code, string? category, string? productId);
        Task<ValidateCouponResponse> ValidateCouponAsync(ValidateCouponRequest request);
        Task<bool> DeleteEntry(Guid id);
    }
}
