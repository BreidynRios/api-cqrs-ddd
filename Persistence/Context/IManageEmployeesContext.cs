using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public interface IManageEmployeesContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<PermissionType> PermissionTypes { get; set; }
        DbSet<Permission> Permissions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
