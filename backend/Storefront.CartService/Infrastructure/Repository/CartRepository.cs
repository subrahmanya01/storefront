using Microsoft.EntityFrameworkCore;
using Storefront.CartService.Entities;

namespace Storefront.CartService.Infrastructure.Repository;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Carts.AsNoTracking().Include(x=> x.Items).FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart?> GetByIdAsync(Guid cartId)
    {
        return await _context.Carts.AsNoTracking().Include(x => x.Items).FirstOrDefaultAsync(x=> x.Id == cartId);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<bool> DeleteAsync(Guid cartId)
    {
        var cart = await _context.Carts.FindAsync(cartId);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
