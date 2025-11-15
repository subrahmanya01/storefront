using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Infrastructure;
using Storefront.OrderAndShippingService.Models.Request;
using Storefront.OrderAndShippingService.Models.Response;

namespace Storefront.OrderAndShippingService.Services
{
    public class ShippingChargeService : IShippingChargeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShippingChargeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShippingChargeResponse> CreateAsync(ShippingChargeRequest request)
        {
            var entity = _mapper.Map<ShippingCharge>(request);
            entity.Id = Guid.NewGuid();
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShippingChargeResponse>(entity);
        }

        public async Task<ShippingChargeResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<ShippingCharge>().FindAsync(id);
            return entity == null ? null : _mapper.Map<ShippingChargeResponse>(entity);
        }

        public async Task<IEnumerable<ShippingChargeResponse>> GetAllAsync()
        {
            var items = await _context.Set<ShippingCharge>().ToListAsync();
            return _mapper.Map<IEnumerable<ShippingChargeResponse>>(items);
        }

        public async Task<ShippingChargeResponse?> UpdateAsync(Guid id, ShippingChargeRequest request)
        {
            var entity = await _context.Set<ShippingCharge>().FindAsync(id);
            if (entity == null) return null;

            _mapper.Map(request, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ShippingChargeResponse>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<ShippingCharge>().FindAsync(id);
            if (entity == null) return false;

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetPostalCodes()
        {
            var postalCodes = _context.ShippingCharges.AsNoTracking().Select(x => x.Region);
            return postalCodes.ToList()!;
        }
    }

}
