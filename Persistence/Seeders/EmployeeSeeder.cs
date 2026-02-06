using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Seeders
{
    public class EmployeeSeeder : IDataSeeder
    {
        private readonly IManageEmployeesContext _context;

        public EmployeeSeeder(IManageEmployeesContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!await _context.Employees.AnyAsync())
            {
                var employees = new[]
                {
                    new Employee
                    { 
                        Name = "Alan",
                        Surname = "Rios",
                        DocumentNumber = "47544850",
                        CreatedBy = 1,
                        CreatedDateOnUtc = DateTime.UtcNow
                    },
                    new Employee
                    {
                        Name = "Juan",
                        Surname = "Perez",
                        DocumentNumber = "12345678",
                        CreatedBy = 1,
                        CreatedDateOnUtc = DateTime.UtcNow
                    },
                };

                await _context.Employees.AddRangeAsync(employees);
                await _context.SaveChangesAsync();
            }
        }
    }
}
