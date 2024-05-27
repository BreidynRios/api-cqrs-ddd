using Domain.Repositories;
using Persistence.Context;

namespace Persistence.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ManageEmployeesContext _context;
        public UnitOfWork(ManageEmployeesContext context)
        {
            _context = context;
        }

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
