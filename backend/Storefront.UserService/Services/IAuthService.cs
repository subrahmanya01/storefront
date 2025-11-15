using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Response;

namespace Storefront.UserService.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> HandleLogin(LoginRequest request);
        Task<AuthResponse> HandleRegister(RegisterRequest request);
        Task<AuthResponse> CompleteRegistration(CompleteRegistrationRequest request);
        Task<AuthResponse> HandleRefreshToken(RefreshTokenRequest request);
        Task<bool> HandleForgotPassword(ForgotPasswordRequest request);
        Task<bool> ResetPassword(Guid id, ResetPasswordRequest request);
    }
}
