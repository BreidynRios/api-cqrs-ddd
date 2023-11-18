using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Commons;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class EmployeeRepository 
        : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ManageEmployeesContext context) : base(context)
        {
        }

        public async Task<Employee> GetEmployeeWithPermissionsAsync(
            int id, CancellationToken cancellationToken)
        {
            return await _entities
                .Include(e => e.Permissions)
                    .ThenInclude(e => e.PermissionType)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
