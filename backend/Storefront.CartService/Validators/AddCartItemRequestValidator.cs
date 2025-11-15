using FluentValidation;
using Storefront.CartService.Models.Request;

namespace Storefront.CartService.Validators
{
    public class AddCartItemRequestValidator : AbstractValidator<AddCartItemRequest>
    {
        public AddCartItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
               .NotNull().NotEmpty()
               .WithMessage("Product is required");
            RuleFor(x => x.ProductVariantId)
               .NotNull().NotEmpty()
               .WithMessage("Product varient id is required");
            RuleFor(x => x.Quantity)
               .NotNull().NotEmpty()
               .WithMessage("Quantity is required")
               .GreaterThanOrEqualTo(0)
               .WithMessage("Quantity should be greater than or equal to 0");
        }
    }
}
