using Storefront.UserService.Entities;

namespace Storefront.UserService.Services
{
    public interface IJwtProvider
    {
        public int JwtRefreshExpirationInHours { get; }
        string Genarate(User user, out DateTime expiresAt);
        string GenarateRefreshToken();
    }
}
