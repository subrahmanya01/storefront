namespace Storefront.UserService.Models.Request
{
    public class RefreshTokenRequest
    {
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
