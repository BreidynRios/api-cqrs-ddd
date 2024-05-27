using Domain.Common.Interfaces;

namespace Domain.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> PagedQueryable(IQueryable<T> set, IPagination pagination);
    }
}
