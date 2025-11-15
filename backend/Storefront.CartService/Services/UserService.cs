using Storefront.CartService.Behaviours;

namespace Storefront.CartService.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetUserId()
        {
            var userInfo = _contextAccessor.HttpContext?.User;
            var userIdClaim = userInfo?.FindFirst("UserId")?.Value;
            if (userIdClaim == null) {
                throw new UnidentifiedUserException();
            }
            return Guid.Parse(userIdClaim);
        }
    }
}
