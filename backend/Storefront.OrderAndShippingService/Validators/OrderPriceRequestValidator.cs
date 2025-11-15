using FluentValidation;
using Storefront.OrderAndShippingService.Models.Request;

namespace Storefront.OrderAndShippingService.Validators
{
    public class OrderPriceRequestValidator : AbstractValidator<OrderPriceRequest>
    {
        public OrderPriceRequestValidator()
        {
            
        }
    }
}
