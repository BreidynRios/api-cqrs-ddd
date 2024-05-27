using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Interceptors;
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
            services.AddSingleton<AuditableEntityInterceptor>();
            services.AddDbContext<IManageEmployeesContext, ManageEmployeesContext>(
                (sp, options) => options
                    .UseSqlServer(
                        configuration.GetConnectionString("SqlConnection"),
                        builder => builder.MigrationsAssembly(typeof(ManageEmployeesContext).Assembly.FullName))
                    .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>()));

        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IRepository<>), typeof(BaseRepository<>))
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>()
                .AddTransient<IPermissionRepository, PermissionRepository>()
                .AddTransient<IPermissionTypeRepository, PermissionTypeRepository>();
        }
    }
}
