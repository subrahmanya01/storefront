using FluentValidation;
using Storefront.UserService.Models.Request;

namespace Storefront.UserService.Validators
{
    public class ContactRequestValidator : AbstractValidator<ContactRequest>
    {
        public ContactRequestValidator()
        {
            RuleFor(item => item.Name)
                .NotEmpty()
                .WithMessage("Name should not be empty");
            RuleFor(item => item.Email)
                .NotEmpty()
                .WithMessage("Email should not be empty")
                .EmailAddress()
                .WithMessage("Invalid email provided");
            RuleFor(item => item.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number should not be empty");
            RuleFor(item => item.Message)
                .NotEmpty().MinimumLength(25)
                .WithMessage("Messae should not be empty and should contain atleast 25 charecters");
        }
    }
}
