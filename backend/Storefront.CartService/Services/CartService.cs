using AutoMapper;
using Storefront.CartService.Entities;
using Storefront.CartService.Infrastructure.Repository;
using Storefront.CartService.Models;
using Storefront.CartService.Models.Request;
using Storefront.CartService.Models.Response;
using Storefront.CartService.Services.GrpcClients;

namespace Storefront.CartService.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductGrpcClient _productGrpcClient;
        private readonly IMapper _mapper;
        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IMapper mapper, IProductGrpcClient productGrpcClient)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _productGrpcClient = productGrpcClient;
        }

        public async Task<CartResponse> GetCartItemsAsync(Guid userId)
        {
            var response = await _cartRepository.GetByUserIdAsync(userId);
            return _mapper.Map<CartResponse>(response);
        }

        public async Task<IResult<Cart>> GetCartById(Guid id)
        {
            var result = await _cartRepository.GetByIdAsync(id);
            if(result == null)
            {
                return Result<Cart>.Fail("Cart not found", 404);
            }
            return Result<Cart>.Ok(result);
        }

        public async Task<IResult<bool>> AddItemToCartAsync(Guid userId, AddCartItemRequest cartItem)
        {
            // Step 1: Validate Product
            var existingProduct = await _productGrpcClient.GetProductByIdAsync(cartItem.ProductId);
            if (existingProduct == null)
            {
                return Result<bool>.Fail($"Product with ID {cartItem.ProductId} not found.", StatusCodes.Status404NotFound);
            }

            var variant = existingProduct.Variants.FirstOrDefault(v => v.Id == cartItem.ProductVariantId);
            if (variant == null)
            {
                return Result<bool>.Fail($"Variant with ID {cartItem.ProductVariantId} not found.", StatusCodes.Status404NotFound);
            }

            // Step 2: Get or create user's cart
            var userCart = await _cartRepository.GetByUserIdAsync(userId);
            if (userCart == null)
            {
                userCart = await _cartRepository.CreateAsync(new Cart { UserId = userId });
            }

            // Step 3: Check if the same product + variant already exists
            var existingItems = await _cartItemRepository.GetByCartIdAsync(userCart.Id);
            var existingItem = existingItems.FirstOrDefault(i =>
                i.ProductId == cartItem.ProductId && i.VariantId == cartItem.ProductVariantId);

            if (existingItem != null)
            {
                // Update quantity if already exists
                existingItem.Quantity = cartItem.Quantity;
                existingItem.Price = (double)variant.Price; // Optional: Update price
                await _cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                // Add new cart item
                var newItem = _mapper.Map<CartItem>(cartItem);
                newItem.CartId = userCart.Id;
                newItem.VariantId = cartItem.ProductVariantId;
                newItem.Price = (double)variant.Price;
                await _cartItemRepository.AddAsync(newItem);
            }

            return Result<bool>.Ok(true);
        }


        public async Task<CartItemResponse?> UpdateCartItemQuantityAsync(Guid itemId, int quantity)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(itemId);
            if (cartItem == null) {
                return null;
            }
            cartItem.Quantity = quantity;
            var updatedCartItem = await _cartItemRepository.UpdateAsync(cartItem);
            return _mapper.Map<CartItemResponse>(updatedCartItem);
        }

        public async Task<bool> RemoveItemFromCartAsync(Guid itemId)
        {
            await _cartItemRepository.RemoveAsync(itemId);
            return true;
        }
        public async Task<bool> ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) { 
                return false;
            }
            await _cartRepository.DeleteAsync(cart.Id);
            return true;
        }
    }

}
