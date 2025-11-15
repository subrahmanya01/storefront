using Storefront.OrderAndShippingService.Entities;
using System.Linq.Expressions;

namespace Storefront.OrderAndShippingService.Infrastructure.Repository
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid orderId);

        Task<Order?> UpdateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId);

        Task<IEnumerable<Order>> GetAsync(Expression<Func<Order, bool>> predicate);
    }

}
