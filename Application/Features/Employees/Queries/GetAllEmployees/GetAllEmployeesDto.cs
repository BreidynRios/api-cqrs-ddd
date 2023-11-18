using Application.Commons.Mappings;
using Domain.Entities;

namespace Application.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesDto : IMapFrom<Employee>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
    }
}
