using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ManageEmployeesContext context) : base(context)
        {
        }

        public async Task<Employee?> GetEmployeeWithPermissionsAsync(
            int id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(e => e.Permissions)
                    .ThenInclude(e => e.PermissionType)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
