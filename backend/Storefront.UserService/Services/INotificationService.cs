using Storefront.UserService.Models.Request;

namespace Storefront.UserService.Services
{
    public interface INotificationService
    {
        Task<bool> SendContactEmail(ContactRequest request);
        Task<bool> SendResetPasswordEmail(string email, string UniqueIdentifier);
        Task<bool> SendVerifyEmailMail(string email, string UniqueIdentifier);
    }
}
