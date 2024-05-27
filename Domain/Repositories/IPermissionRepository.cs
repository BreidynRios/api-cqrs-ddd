using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission?> GetPermissionWithEmployeeTypeAsync(int id, CancellationToken cancellationToken);
    }
}
