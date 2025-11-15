using AutoMapper;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Models.Response;
using Storefront.ProductService.Protos;

namespace Storefront.ProductService.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
                .ForMember(dest => dest.AllowedAttributes, opt => opt.MapFrom(src => src.AllowedAttributes))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants ?? new List<ProductVariant>()));

            CreateMap<ProductVariant, ProductVariantMessage>()
                .ForMember(dest => dest.IsComplete, opt => opt.MapFrom(src => src.Images != null && src.Images.Count > 0));

            CreateMap<HashSet<string>, AttributeValuesList>()
                .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.ToList()));

            CreateMap<Dictionary<string, HashSet<string>>, Dictionary<string, AttributeValuesList>>()
                .ConvertUsing((src, dest, context) =>
                {
                    var dict = new Dictionary<string, AttributeValuesList>();
                    foreach (var kvp in src)
                    {
                        dict[kvp.Key] = context.Mapper.Map<AttributeValuesList>(kvp.Value);
                    }
                    return dict;
                });
            CreateMap<FlashSaleRequestModel, FlashSale>().ReverseMap();
            CreateMap<Product, ProductAddResponse>().ReverseMap();
        }
    }
}
