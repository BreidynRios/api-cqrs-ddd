using MediatR;

namespace Application.Features.Employees.Queries.GetAllEmployees
{
    public record GetAllEmployeesQuery : IRequest<IEnumerable<GetAllEmployeesDto>>;
}
