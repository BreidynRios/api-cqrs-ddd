using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<Permission> GetPermissionWithEmployeeTypeAsync(int id, CancellationToken cancellationToken);
    }
}
