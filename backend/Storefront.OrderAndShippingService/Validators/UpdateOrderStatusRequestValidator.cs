using FluentValidation;
using Storefront.OrderAndShippingService.Models.Request;
using System.Data;

namespace Storefront.OrderAndShippingService.Validators
{
    public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
    {
        public UpdateOrderStatusRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().NotNull()
                .WithMessage("Order id should not be null or empty");
            RuleFor(x => x.OrderStatus)
                .NotEmpty().NotNull()
                .WithMessage("Order id should not be null or empty");
        }
    }
}
