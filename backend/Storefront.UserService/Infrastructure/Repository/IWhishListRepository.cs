using Storefront.UserService.Entities;

namespace Storefront.UserService.Infrastructure.Repository
{
    public interface IWhishListRepository
    {
        Task<WhishList> GetOrCreateByUserIdAsync(Guid userId);
        Task<IEnumerable<WhishListItem>> GetItemsByUserIdAsync(Guid userId);
        Task AddItemByUserIdAsync(Guid userId, string productId);
        Task RemoveItemByUserIdAsync(Guid userId, string productId);
        Task ClearByUserIdAsync(Guid userId);
    }
}
