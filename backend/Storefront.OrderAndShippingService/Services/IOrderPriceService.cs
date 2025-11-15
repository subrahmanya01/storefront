using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public interface IOrderPriceService
    {
        Task<OrderPriceResponse> CalculateOrderPrice(OrderPriceRequest request);
        Task<double> GetTotalAmount(OrderPriceResponse priceInfo);

    }
}
