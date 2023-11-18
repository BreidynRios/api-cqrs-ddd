using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee> GetEmployeeWithPermissionsAsync(int id, CancellationToken cancellationToken);
    }
}
