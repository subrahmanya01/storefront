using FluentValidation;
using Storefront.ProductService.Models.Request;

namespace Storefront.ProductService.Validators
{
    public class FlashSaleRequestModelValidator : AbstractValidator<FlashSaleRequestModel>
    {
        public FlashSaleRequestModelValidator()
        {
            RuleFor(p => p.ProductId)
                .NotEmpty().WithMessage("Product Id is required");
            RuleFor(p => p.StartsAt)
                .NotEmpty().NotNull().WithMessage("Start at is required");
            RuleFor(p => p.EndsAt)
                .NotEmpty().NotNull().WithMessage("Ends at is required");
            RuleFor(p => p.EndsAt)
                .Must((model, endsAt) => endsAt > model.StartsAt)
                .WithMessage("EndsAt must be greater than StartsAt");
        }
    }
}
