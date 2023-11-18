using Application.Commons.Mappings;
using Domain.Entities;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdDto : IMapFrom<Permission>
    {
        public GetPermissionEmployeDto Employee { get; set; }
        public GetPermissionTypeDto PermissionType { get; set; }
    }

    public class GetPermissionEmployeDto : IMapFrom<Employee> 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class GetPermissionTypeDto : IMapFrom<PermissionType>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
