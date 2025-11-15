using Storefront.CartService.Entities;
using Storefront.CartService.Models;
using Storefront.CartService.Models.Request;
using Storefront.CartService.Models.Response;

namespace Storefront.CartService.Services
{
    public interface ICartService
    {
        Task<CartResponse> GetCartItemsAsync(Guid userId);
        Task<IResult<bool>> AddItemToCartAsync(Guid userId, AddCartItemRequest cartItem);
        Task<CartItemResponse?> UpdateCartItemQuantityAsync(Guid itemId, int quantity);
        Task<bool> RemoveItemFromCartAsync(Guid itemId);
        Task<bool> ClearCartAsync(Guid userId);
        Task<IResult<Cart>> GetCartById(Guid id);
    }
}
