using AutoMapper;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;
using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Mappings
{
    public class ShippingChargeProfile : Profile
    {
        public ShippingChargeProfile()
        {
            CreateMap<ShippingCharge, ShippingChargeRequest>().ReverseMap();
            CreateMap<ShippingCharge, ShippingChargeResponse>().ReverseMap();

            CreateMap<ShippingChargeResponse, ShippingChargeRpcResponse>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country ?? ""))
           .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region ?? ""))
           .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId ?? ""))
           .ForMember(dest => dest.MinOrderAmount, opt => opt.MapFrom(src => Convert.ToDouble(src.MinOrderAmount)))
           .ForMember(dest => dest.MaxOrderAmount, opt => opt.MapFrom(src => Convert.ToDouble(src.MaxOrderAmount ?? 0)))
           .ForMember(dest => dest.ShippingFeePerKm, opt => opt.MapFrom(src => Convert.ToDouble(src.ShippingFeePerKm)))
           .ForMember(dest => dest.IsFree, opt => opt.MapFrom(src => src.IsFree))
           .ForMember(dest => dest.Carrier, opt => opt.MapFrom(src => src.Carrier ?? ""))
           .ForMember(dest => dest.EffectiveFrom, opt => opt.MapFrom(src => src.EffectiveFrom.HasValue ? src.EffectiveFrom.Value.ToString("o") : ""))
           .ForMember(dest => dest.EffectiveTo, opt => opt.MapFrom(src => src.EffectiveTo.HasValue ? src.EffectiveTo.Value.ToString("o") : ""));
        }
    }
}
