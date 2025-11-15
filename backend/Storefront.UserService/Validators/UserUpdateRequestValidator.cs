using FluentValidation;
using Storefront.UserService.Models.Request;

namespace Storefront.UserService.Validators
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name should not be empty");

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
        }
    }
}
