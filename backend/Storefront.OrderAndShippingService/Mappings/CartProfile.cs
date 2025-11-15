using AutoMapper;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile() {
            CreateMap<CartItemRpcResponse, OrderItem>()
                 .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));
        }
    }
}
