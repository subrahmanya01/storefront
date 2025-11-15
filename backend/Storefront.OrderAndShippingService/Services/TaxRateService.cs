using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Infrastructure;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public class TaxRateService : ITaxRateService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaxRateService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaxRateResponse> CreateAsync(TaxRateRequest request)
        {
            var existing = await _context.TaxRates.AsNoTracking().FirstOrDefaultAsync(x => x.Category == request.Category);
            if (existing != null)
            {
                return null!;
            }

            var taxRate = _mapper.Map<TaxRate>(request);
            taxRate.Id = Guid.NewGuid();
            _context.Set<TaxRate>().Add(taxRate);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaxRateResponse>(taxRate);
        }

        public async Task<TaxRateResponse?> GetByIdAsync(Guid id)
        {
            var taxRate = await _context.Set<TaxRate>().FindAsync(id);
            return taxRate is null ? null : _mapper.Map<TaxRateResponse>(taxRate);
        }

        public async Task<IEnumerable<TaxRateResponse>> GetAllAsync()
        {
            var rates = await _context.Set<TaxRate>().ToListAsync();
            return _mapper.Map<IEnumerable<TaxRateResponse>>(rates);
        }

        public async Task<TaxRateResponse?> UpdateAsync(Guid id, TaxRateRequest request)
        {
            var taxRate = await _context.Set<TaxRate>().FindAsync(id);
            if (taxRate == null) return null;

            _mapper.Map(request, taxRate);
            _context.Update(taxRate);
            await _context.SaveChangesAsync();
            return _mapper.Map<TaxRateResponse>(taxRate);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var taxRate = await _context.Set<TaxRate>().FindAsync(id);
            if (taxRate == null) return false;

            _context.Remove(taxRate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
