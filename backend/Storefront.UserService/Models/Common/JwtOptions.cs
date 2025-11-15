namespace Storefront.UserService.Models.Common
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int JwtExpirationInMinutes { get; set; } 
        public int JwtRefreshExpirationInHours { get; set; }
        public string SecretToken { get; set; } = string.Empty;
    }
}
