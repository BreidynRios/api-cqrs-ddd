using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithPermissionsAsync(int id, CancellationToken cancellationToken);
    }
}
