using Application.Interfaces.Repositories;

namespace Application.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPermissionTypeRepository PermissionTypeRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
