using Storefront.CartService.Entities;

namespace Storefront.CartService.Infrastructure.Repository
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart?> GetByIdAsync(Guid cartId);
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
        Task<bool> DeleteAsync(Guid cartId);
    }
}
