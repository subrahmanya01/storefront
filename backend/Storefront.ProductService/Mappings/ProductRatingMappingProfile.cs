using AutoMapper;
using DnsClient;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Models.Response;

namespace Storefront.ProductService.Mappings
{
    public class ProductRatingMappingProfile : Profile
    {
        public ProductRatingMappingProfile()
        {
            CreateMap<ProductRating, RatingResponse>();
        }
    }
}
