using Microsoft.EntityFrameworkCore;

namespace Storefront.OrderAndShippingService.Entities
{
    [Owned]
    public class ShippingAddress
    {
        public string FullName { get; set; } = default!;
        public string Line1 { get; set; } = default!;
        public string? Line2 { get; set; }
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
