using MediatR;

namespace Application.Features.Employees.Queries.ExportEmployeesBackgroundJob
{
    public class ExportEmployeesBackgroundJobQuery : IRequest<string>
    {
        public string FilterName { get; set; }
        public string FilterSurname { get; set; }
        public IEnumerable<int> PermissionsTypesIds { get; set; }
    }
}
