using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Context
{
    public class ManageEmployeesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ManageEmployeesContext>
    {
        public ManageEmployeesContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebApi"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.Docker.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ManageEmployeesContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("SqlConnection"));

            return new ManageEmployeesContext(optionsBuilder.Options);
        }
    }
}
