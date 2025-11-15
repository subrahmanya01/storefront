using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Infrastructure;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;
using System;

namespace Storefront.OrderAndShippingService.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DiscountService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        public async Task<Discount> CreateDiscountAsync(DiscountRequest discount)
        {
            var mappedDiscount = _mapper.Map<Discount>(discount);
            _context.Discounts.Add(mappedDiscount);
            await _context.SaveChangesAsync();
            return mappedDiscount;
        }

        public async Task<Discount?> UpdateDiscountAsync(Guid id, DiscountRequest updated)
        {
            var existing = await _context.Discounts.FindAsync(id);
            if (existing == null)
                return null;

            existing.Code = updated.Code;
            existing.Percentage = updated.Percentage;
            existing.MinOrderAmount = updated.MinOrderAmount;
            existing.Category = updated.Category;
            existing.ProductId = updated.ProductId;
            existing.ValidFrom = updated.ValidFrom;
            existing.ValidTo = updated.ValidTo;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<List<Discount>> GetAllDiscountsAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<Discount?> GetDiscountByIdAsync(Guid id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        public async Task<List<Discount>> SearchDiscountsAsync(string? code, string? category, string? productId)
        {
            var query = _context.Discounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(d => d.Code == code);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(d => d.Category == category);

            if (!string.IsNullOrWhiteSpace(productId))
                query = query.Where(d => d.ProductId == productId);

            return await query.ToListAsync();
        }

        public async Task<ValidateCouponResponse> ValidateCouponAsync(ValidateCouponRequest request)
        {
            var now = DateTime.UtcNow;

            var coupon = await _context.Discounts
                .Where(d => d.Code == request.Code && d.ValidFrom <= now && d.ValidTo >= now)
                .FirstOrDefaultAsync();

            if (coupon == null)
            {
                return new ValidateCouponResponse
                {
                    IsValid = false,
                    DiscountPercentage = 0,
                    Message = "Coupon not found or expired"
                };
            }

            if (request.OrderAmount < coupon.MinOrderAmount)
            {
                return new ValidateCouponResponse
                {
                    IsValid = false,
                    DiscountPercentage = 0,
                    Message = $"Minimum order amount should be {coupon.MinOrderAmount:C}"
                };
            }

            if (!string.IsNullOrWhiteSpace(coupon.ProductId) &&
                request.ProductId != coupon.ProductId)
            {
                return new ValidateCouponResponse
                {
                    IsValid = false,
                    DiscountPercentage = 0,
                    Message = "Coupon not applicable for this product"
                };
            }

            if (!string.IsNullOrWhiteSpace(coupon.Category) &&
                request.Category != coupon.Category)
            {
                return new ValidateCouponResponse
                {
                    IsValid = false,
                    DiscountPercentage = 0,
                    Message = "Coupon not applicable for this category"
                };
            }

            return new ValidateCouponResponse
            {
                IsValid = true,
                DiscountPercentage = coupon.Percentage,
                Message = "Coupon applied successfully"
            };
        }

        public async Task<bool> DeleteEntry(Guid id)
        {
           var existigEntry = await _context.Discounts.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == id);
            if (existigEntry != null) {
                _context.Discounts.Remove(existigEntry);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }

}
