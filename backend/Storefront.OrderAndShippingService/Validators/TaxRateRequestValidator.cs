using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Infrastructure;
using Storefront.OrderAndShippingService.Models.Request;

namespace Storefront.OrderAndShippingService.Validators
{
    public class TaxRateRequestValidator : AbstractValidator<TaxRateRequest>
    {
        public TaxRateRequestValidator()
        {

        }
    }
}
