using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ManageEmployeesContext context) : base(context)
        {
        }

        public async Task<Permission?> GetPermissionWithEmployeeTypeAsync(
            int id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(p => p.Employee)
                .Include(p => p.PermissionType)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
