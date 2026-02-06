using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Interceptors;
using Persistence.Repositories;
using Persistence.Seeders;

namespace Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
            services.AddSeeders();
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
                .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IEmployeeRepository, EmployeeRepository>()
                .AddScoped<IPermissionRepository, PermissionRepository>()
                .AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
        }

        private static void AddSeeders(this IServiceCollection services)
        {
            services
                .AddScoped<IDataSeeder, PermissionTypeSeeder>()
                .AddScoped<IDataSeeder, EmployeeSeeder>()
                .AddScoped<DefaultDataSeeder>();
        }
    }
}
