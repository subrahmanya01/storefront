using AutoMapper;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Mappings
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountRequest>().ReverseMap();
            CreateMap<Discount, DiscountRpcResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("o")))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt.ToString("o")))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code ?? ""))
            .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => Convert.ToDouble(src.Percentage)))
            .ForMember(dest => dest.MinOrderAmount, opt => opt.MapFrom(src => Convert.ToDouble(src.MinOrderAmount ?? 0)))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category ?? ""))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId ?? ""))
            .ForMember(dest => dest.ValidFrom, opt => opt.MapFrom(src => src.ValidFrom.ToString("o")))
            .ForMember(dest => dest.ValidTo, opt => opt.MapFrom(src => src.ValidTo.ToString("o")));
        }
    }
}
