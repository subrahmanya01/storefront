using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Response;
using Storefront.UserService.Models;

namespace Storefront.UserService.Services
{
    public interface IUserService
    {
        Task<IResult<UserResponse>> UpdateUser(Guid userId, UserUpdateRequest request);
    }
}
