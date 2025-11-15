using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public interface ITaxRateService
    {
        Task<TaxRateResponse> CreateAsync(TaxRateRequest request);
        Task<TaxRateResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaxRateResponse>> GetAllAsync();
        Task<TaxRateResponse?> UpdateAsync(Guid id, TaxRateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
