using Microsoft.EntityFrameworkCore;
using Storefront.UserService.Entities;

namespace Storefront.UserService.Infrastructure.Repository
{
    public class WhishListRepository : IWhishListRepository
    {

        private readonly ApplicationDbContext _context;

        public WhishListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WhishList> GetOrCreateByUserIdAsync(Guid userId)
        {
            var whishList = await _context.WhishLists
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (whishList == null)
            {
                whishList = new WhishList
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };
                _context.WhishLists.Add(whishList);
                await _context.SaveChangesAsync();
            }

            return whishList;
        }

        public async Task<IEnumerable<WhishListItem>> GetItemsByUserIdAsync(Guid userId)
        {
            var wishList = await _context.WhishLists
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishList == null)
                return Enumerable.Empty<WhishListItem>();

            return await _context.WhishListItems
                .Where(i => i.WhishListId == wishList.Id)
                .ToListAsync();
        }

        public async Task AddItemByUserIdAsync(Guid userId, string productId)
        {
            var wishList = await GetOrCreateByUserIdAsync(userId);

            bool exists = await _context.WhishListItems
                .AnyAsync(i => i.WhishListId == wishList.Id && i.ProductId == productId);

            if (!exists)
            {
                _context.WhishListItems.Add(new WhishListItem
                {
                    Id = Guid.NewGuid(),
                    WhishListId = wishList.Id,
                    ProductId = productId
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveItemByUserIdAsync(Guid userId, string productId)
        {
            var wishList = await _context.WhishLists
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishList == null)
                return;

            var item = await _context.WhishListItems
                .FirstOrDefaultAsync(i => i.WhishListId == wishList.Id && i.ProductId == productId);

            if (item != null)
            {
                _context.WhishListItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearByUserIdAsync(Guid userId)
        {
            var wishList = await _context.WhishLists
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishList == null)
                return;

            var items = _context.WhishListItems
                .Where(i => i.WhishListId == wishList.Id);

            _context.WhishListItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
