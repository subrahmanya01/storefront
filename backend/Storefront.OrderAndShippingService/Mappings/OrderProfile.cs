using AutoMapper;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));
            CreateMap<OrderItem, OrderItemResponse>();
        }
    }

}
