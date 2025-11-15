using FluentValidation;
using Storefront.CartService.Models.Request;

namespace Storefront.CartService.Validators
{
    public class UpdateQuantityRequestValidator : AbstractValidator<UpdateQuantityRequest>
    {
        public UpdateQuantityRequestValidator()
        {
            RuleFor(x => x.Quantity)
               .NotNull().NotEmpty()
               .WithMessage("Quantity is required")
               .GreaterThanOrEqualTo(0)
               .WithMessage("Quantity should be greater than or equal to 0");
        }
    }
}
