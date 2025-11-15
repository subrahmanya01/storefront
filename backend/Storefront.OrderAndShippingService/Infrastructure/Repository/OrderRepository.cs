using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;
using System;
using System.Linq.Expressions;

namespace Storefront.OrderAndShippingService.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAsync(Expression<Func<Order, bool>> predicate)
        {
            if (predicate == null) { 
              throw new ArgumentNullException(nameof(predicate));
            }
            return await _context.Orders.AsNoTracking().Include(o => o.Items).Where(predicate).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            return await _context.Orders.Include(o => o.Items).Where(o => o.CustomerId == customerId).ToListAsync();
        }
    }

}
