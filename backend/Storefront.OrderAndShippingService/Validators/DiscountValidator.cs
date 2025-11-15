namespace Storefront.OrderAndShippingService.Validators
{
    using FluentValidation;
    using Storefront.OrderAndShippingService.Models.Request;

    public class DiscountRequestValidator : AbstractValidator<DiscountRequest>
    {
        public DiscountRequestValidator()
        {
            // Percentage is required and must be 0–100
            RuleFor(x => x.Percentage)
                .GreaterThanOrEqualTo(0).WithMessage("Percentage must be greater than or equal to 0.")
                .LessThanOrEqualTo(100).WithMessage("Percentage must be less than or equal to 100.");

            // ValidFrom and ValidTo are required and logically correct
            RuleFor(x => x.ValidFrom)
                .NotEmpty().WithMessage("ValidFrom is required.")
                .LessThanOrEqualTo(x => x.ValidTo).WithMessage("ValidFrom must be before or equal to ValidTo.");

            RuleFor(x => x.ValidTo)
                .NotEmpty().WithMessage("ValidTo is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("ValidTo must be in the future.");

            // Conditional validation: if Code is provided, limit its length
            When(x => !string.IsNullOrWhiteSpace(x.Code), () =>
            {
                RuleFor(x => x.Code)
                    .MaximumLength(50)
                    .WithMessage("Code must be at most 50 characters long.");
            });

            // If MinOrderAmount is provided, it must be >= 0
            When(x => x.MinOrderAmount.HasValue, () =>
            {
                RuleFor(x => x.MinOrderAmount!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("MinOrderAmount must be non-negative.");
            });

            // At least one of Code, Category, ProductId, MinOrderAmount must be set
            RuleFor(x => x)
                .Must(HaveAtLeastOneOptionalField)
                .WithMessage("At least one of Code, Category, ProductId, or MinOrderAmount must be provided.");
        }

        private bool HaveAtLeastOneOptionalField(DiscountRequest x)
        {
            return !string.IsNullOrWhiteSpace(x.Code)
                || !string.IsNullOrWhiteSpace(x.Category)
                || !string.IsNullOrWhiteSpace(x.ProductId)
                || x.MinOrderAmount.HasValue;
        }
    }



}
