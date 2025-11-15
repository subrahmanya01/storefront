using FluentValidation;
using Storefront.UserService.Models.Request;

namespace Storefront.UserService.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .NotEmpty()
                .WithMessage("Refresh token should not be empty or null");
        }
    }
}
