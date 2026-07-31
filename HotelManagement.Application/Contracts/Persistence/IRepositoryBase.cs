
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelManagement.Application.Contracts.Persistence
{
    public interface IRepositoryBase<T, TContext>
        where T : class
        where TContext : DbContext
    {
        Task<(IEnumerable<T> items, int TotalCount)> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            int? pageSize = null,
            int? pageNumber = null,
            Expression<Func<T, object>> orderBy = null,
            bool ascending = true,
            CancellationToken cancellationToken = default,
            bool tracking = true,
            params Expression<Func<T, object>>[] includes);

        Task<T?> GetAsync(
            Expression<Func<T, bool>> filter,
            bool ascending = true,
            CancellationToken cancellationToken = default,
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        Task AddAsync(T entity);
        Task<int> SaveAsync(CancellationToken cancellationToken);
        void Remove(T entity);
        void Update(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
