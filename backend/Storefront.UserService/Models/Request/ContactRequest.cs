namespace Storefront.UserService.Models.Request
{
    public class ContactRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
    }
}
