using Microsoft.EntityFrameworkCore;
using Storefront.CartService.Entities;

namespace Storefront.CartService.Infrastructure.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetByIdAsync(Guid itemId)
        {
            return await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == itemId);
        }

        public async Task<CartItem> AddAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CartItem> UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoveAsync(Guid itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<CartItem>> GetByCartIdAsync(Guid cartId)
        {
            return await _context.CartItems.AsNoTracking()
                .Where(item => item.CartId == cartId)
                .ToListAsync();
        }

        public async Task<bool> ClearItemsAsync(Guid cartId)
        {
            var items = await _context.CartItems
                .Where(item => item.CartId == cartId)
                .ToListAsync();

            if(items.Any())
            {
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
