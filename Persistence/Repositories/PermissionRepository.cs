using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Commons;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class PermissionRepository
        : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ManageEmployeesContext context) : base(context)
        {
        }

        public async Task<Permission> GetPermissionWithEmployeeTypeAsync(
            int id, CancellationToken cancellationToken)
        {
            return await _entities
                .Include(p => p.Employee)
                .Include(p => p.PermissionType)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
