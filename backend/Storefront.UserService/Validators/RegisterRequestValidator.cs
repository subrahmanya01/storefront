using FluentValidation;
using Storefront.UserService.Models.Request;

namespace Storefront.UserService.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{1,4}[-\s]?\d{6,15}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Phone number must include a valid country code and number.");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Either email or phone number must be provided.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
