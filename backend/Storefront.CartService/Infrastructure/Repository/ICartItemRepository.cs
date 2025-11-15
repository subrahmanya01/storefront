using Storefront.CartService.Entities;

namespace Storefront.CartService.Infrastructure.Repository
{
    public interface ICartItemRepository
    {
        Task<CartItem?> GetByIdAsync(Guid itemId);
        Task<CartItem> AddAsync(CartItem item);  
        Task<CartItem> UpdateAsync(CartItem item);
        Task<bool> RemoveAsync(Guid itemId);
        Task<List<CartItem>> GetByCartIdAsync(Guid cartId);
        Task<bool> ClearItemsAsync(Guid cartId);
    }
}
