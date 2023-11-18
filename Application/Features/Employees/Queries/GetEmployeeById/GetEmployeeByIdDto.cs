using Application.Commons.Mappings;
using Domain.Entities;

namespace Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdDto : IMapFrom<Employee>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
        public ICollection<GetEmployeePermissionDto> Permissions { get; set; }
    }

    public class GetEmployeePermissionDto : IMapFrom<Permission>
    {
        public int Id { get; set; }
        public GetEmployeeTypePermissionDto PermissionType { get; set; }
    }

    public class GetEmployeeTypePermissionDto : IMapFrom<PermissionType>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
