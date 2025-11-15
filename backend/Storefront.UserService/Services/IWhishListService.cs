using Storefront.UserService.Entities;

namespace Storefront.UserService.Services
{
    public interface IWhishListService
    {
        Task<IEnumerable<WhishListItem>> GetItemsAsync(Guid userId);
        Task AddItemAsync(Guid userId, string productId);
        Task RemoveItemAsync(Guid userId, string productId);
        Task ClearAsync(Guid userId);
    }
}
