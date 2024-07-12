using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected DbSet<T> _dbSet;
        public BaseRepository(ManageEmployeesContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
            => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await _dbSet.FindAsync(id, cancellationToken);

        public IQueryable<T> PagedQueryable(IQueryable<T> set, IPagination pagination)
        {
            if (!pagination.IsValid()) return set;

            set.Skip(pagination.PageSize!.Value * pagination.Page!.Value)
                .Take(pagination.PageSize.Value);

            return set;
        }
    }
}
