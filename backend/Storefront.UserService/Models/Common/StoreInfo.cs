namespace Storefront.UserService.Models.Common
{
    public class StoreInfo
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Website { get; set; }
        public List<string>? CompanySupportEmails { get; set; }
        public List<string>? PhoneNumber { get; set; }
        public string? TagLine { get; set; }
        public string? MainEmail { get; set; }
        public SocialMediaLinks? SocialMediaLinks { get; set; }
    }

    public class SocialMediaLinks
    {
        public string? Facebook { get; set; }
        public string? LinkedIn { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
    }
}
