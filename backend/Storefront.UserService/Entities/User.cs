using Microsoft.EntityFrameworkCore;

namespace Storefront.UserService.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Id), IsUnique = true)]

    public class User : EntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } = "User";
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
