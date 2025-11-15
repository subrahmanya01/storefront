using FluentValidation;
using Storefront.ProductService.Entities;

namespace Storefront.ProductService.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required");

            RuleFor(p => p.Type)
                .NotEmpty().WithMessage("Product type is required");

            RuleFor(p => p.Category)
               .NotEmpty().WithMessage("Product category is required");

            RuleFor(p => p.AllowedAttributes)
                .Must(attrs => attrs.All(a => !string.IsNullOrWhiteSpace(a)))
                .WithMessage("AllowedAttributes must not contain empty values");

            RuleForEach(p => p.Variants)
                .Must((product, variant) =>
                    variant.Attributes == null ||
                    variant.Attributes.Keys.All(key => product.AllowedAttributes.Contains(key)))
                .WithMessage("One or more variant attributes are not allowed");

            RuleForEach(p => p.Variants)
                .SetValidator(new ProductVariantValidator());

            RuleFor(v => v.Id)
                .Custom((id, context) =>
                {
                    if (context.RootContextData.TryGetValue("Mode", out var modeObj))
                    {
                        var mode = modeObj as string;

                        if (mode == "create" && !string.IsNullOrEmpty(id))
                        {
                            context.AddFailure("Id should not be set on create.");
                        }

                        if (mode == "update" && string.IsNullOrEmpty(id))
                        {
                            context.AddFailure("Id is required on update.");
                        }
                    }
                });
        }
    }
}
