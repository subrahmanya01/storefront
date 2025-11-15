using AutoMapper;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Mappings
{
    public class TaxRateProfile : Profile
    {
        public TaxRateProfile()
        {
            CreateMap<TaxRate, TaxRateResponse>().ReverseMap();
            CreateMap<TaxRate, TaxRateRequest>().ReverseMap();
        }
    }
}
