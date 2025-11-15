using FluentValidation;
using Storefront.ProductService.Entities;

namespace Storefront.ProductService.Validators
{
    public class ProductVariantValidator : AbstractValidator<ProductVariant>
    {
        public ProductVariantValidator()
        {
            RuleFor(v => v.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be non-negative");

            RuleFor(v => v.Inventory)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Inventory must be non-negative");

            RuleFor(v => v.Id)
                .Custom((id, context) =>
                {
                    if (context.RootContextData.TryGetValue("Mode", out var modeObj))
                    {
                        var mode = modeObj as string;

                        if (mode == "create" && !string.IsNullOrEmpty(id))
                        {
                            context.AddFailure("VariantId should not be set on create.");
                        }

                        if (mode == "update" && string.IsNullOrEmpty(id))
                        {
                            context.AddFailure("VariantId is required on update.");
                        }
                    }
                });
        }
    }
}
