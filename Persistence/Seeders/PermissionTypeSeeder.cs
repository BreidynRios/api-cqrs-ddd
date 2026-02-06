using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Seeders
{
    public class PermissionTypeSeeder : IDataSeeder
    {
        private readonly IManageEmployeesContext _context;

        public PermissionTypeSeeder(IManageEmployeesContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!await _context.PermissionTypes.AnyAsync())
            {
                var types = new[]
                {
                    new PermissionType { Name = "Administrador" },
                    new PermissionType { Name = "Supervisor" },
                    new PermissionType { Name = "Ejecutivo Comercial" },
                    new PermissionType { Name = "Secretaria" },
                    new PermissionType { Name = "Vendedor" }
                };

                await _context.PermissionTypes.AddRangeAsync(types);
                await _context.SaveChangesAsync();
            }
        }
    }
}
