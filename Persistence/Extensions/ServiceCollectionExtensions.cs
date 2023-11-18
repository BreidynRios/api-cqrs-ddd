using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Commons;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ManageEmployeesContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
                builder => builder.MigrationsAssembly(typeof(ManageEmployeesContext).Assembly.FullName)));
            services.AddScoped<IManageEmployeesContext>(provider => provider.GetService<ManageEmployeesContext>());
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>()
                .AddTransient<IPermissionRepository, PermissionRepository>()
                .AddTransient<IPermissionTypeRepository, PermissionTypeRepository>();
        }
    }
}
