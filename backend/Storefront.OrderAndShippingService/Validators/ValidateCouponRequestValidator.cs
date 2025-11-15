using FluentValidation;
using Storefront.OrderAndShippingService.Models.Request;

namespace Storefront.OrderAndShippingService.Validators
{

    public class ValidateCouponRequestValidator : AbstractValidator<ValidateCouponRequest>
    {
        public ValidateCouponRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Coupon code is required.");

            RuleFor(x => x.OrderAmount)
                .GreaterThan(0).WithMessage("Order amount must be greater than 0.");
        }
    }

}
