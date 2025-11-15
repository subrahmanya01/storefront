using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(Guid userId, CreateOrderRequest request);
        Task<OrderResponse?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderResponse>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<OrderResponse>> GetAllOrders();
        Task<OrderResponse?> UpdateOrderStatus(UpdateOrderStatusRequest request);
        Task<bool> CancelOrder(Guid orderId);
    }
}
