using Storefront.UserService.Entities;
using Storefront.UserService.Infrastructure.Repository;

namespace Storefront.UserService.Services
{
    public class WhishListService : IWhishListService
    {
        private readonly IWhishListRepository _repository;

        public WhishListService(IWhishListRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<WhishListItem>> GetItemsAsync(Guid userId)
        {
            return _repository.GetItemsByUserIdAsync(userId);
        }

        public Task AddItemAsync(Guid userId, string productId)
        {
            return _repository.AddItemByUserIdAsync(userId, productId);
        }

        public Task RemoveItemAsync(Guid userId, string productId)
        {
            return _repository.RemoveItemByUserIdAsync(userId, productId);
        }

        public Task ClearAsync(Guid userId)
        {
            return _repository.ClearByUserIdAsync(userId);
        }
    }
}
