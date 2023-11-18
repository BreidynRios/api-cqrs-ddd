using Application.Interfaces.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Commons
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        protected DbSet<T> _entities;
        public GenericRepository(ManageEmployeesContext context)
        {
            _entities = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken);
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken) 
            => await _entities.AsNoTracking().ToListAsync(cancellationToken);

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await _entities.FindAsync(id, cancellationToken);

        public Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            return Task.CompletedTask;
        }
    }
}
