using Storefront.UserService.Entities;
using System.Linq.Expressions;

namespace Storefront.UserService.Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetAsync(Expression<Func<User, bool>> expression);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
    }
}
