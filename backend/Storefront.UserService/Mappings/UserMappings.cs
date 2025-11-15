using AutoMapper;
using Storefront.UserService.Entities;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Response;
using System.Net;

namespace Storefront.UserService.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, User>();
            CreateMap<User, UserResponse>();
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email.ToLower()))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber.Replace(" ", string.Empty)));

            CreateMap<(User user, UserUpdateRequest request), User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.user.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.request.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.request.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.request.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.request.PhoneNumber))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.request.NewPassword) ? src.user.Password : src.request.NewPassword))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.user.Role))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.user.RefreshToken))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.user.CreatedAt));
        }
    }
}
