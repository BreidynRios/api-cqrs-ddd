using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<PermissionType?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<PermissionType>> GetAllAsync(CancellationToken cancellationToken);
    }
}
