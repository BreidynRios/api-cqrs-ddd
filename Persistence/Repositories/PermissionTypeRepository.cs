using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Commons;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class PermissionTypeRepository 
        : GenericRepository<PermissionType>, IPermissionTypeRepository
    {
        public PermissionTypeRepository(ManageEmployeesContext context) : base(context)
        {
        }
    }
}
