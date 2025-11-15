using AutoMapper;
using Storefront.CartService.Entities;
using Storefront.CartService.Models.Request;
using Storefront.CartService.Models.Response;
using Storefront.CartService.Protos;
using Storefront.ProductService.Protos;

namespace Storefront.CartService.Mappings
{
    public class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<AddCartItemRequest, CartItem>();
            CreateMap<Cart, CartResponse>();
            CreateMap<CartItem, CartItemResponse>();
            CreateMap<Cart, CartRpcResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("O")))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt.ToString("O")))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartItem, CartItemRpcResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("O")))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt.ToString("O")))
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId.ToString()))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.VariantId, opt => opt.MapFrom(src => src.VariantId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
