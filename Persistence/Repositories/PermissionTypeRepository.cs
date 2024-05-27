using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly IManageEmployeesContext _context;

        public PermissionTypeRepository(IManageEmployeesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermissionType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.PermissionTypes
                .ToListAsync(cancellationToken);
        }

        public async Task<PermissionType?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.PermissionTypes
                .FindAsync(id, cancellationToken);
        }
    }
}
