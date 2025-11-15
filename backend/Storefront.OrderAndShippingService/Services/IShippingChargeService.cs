using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public interface IShippingChargeService
    {
        Task<ShippingChargeResponse> CreateAsync(ShippingChargeRequest request);
        Task<ShippingChargeResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<ShippingChargeResponse>> GetAllAsync();
        Task<ShippingChargeResponse?> UpdateAsync(Guid id, ShippingChargeRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<List<string>> GetPostalCodes();
    }

}
