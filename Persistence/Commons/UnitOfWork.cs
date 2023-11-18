using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence.Commons
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ManageEmployeesContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        public UnitOfWork(ManageEmployeesContext context)
        {
            _context = context;
        }

        public IEmployeeRepository EmployeeRepository
            => _employeeRepository ?? new EmployeeRepository(_context);
        public IPermissionRepository PermissionRepository
            => _permissionRepository ?? new PermissionRepository(_context);
        public IPermissionTypeRepository PermissionTypeRepository
            => _permissionTypeRepository ?? new PermissionTypeRepository(_context);

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
