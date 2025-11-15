using FluentValidation;
using Storefront.OrderAndShippingService.Models.Request;

namespace Storefront.OrderAndShippingService.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            
        }
    }

}
