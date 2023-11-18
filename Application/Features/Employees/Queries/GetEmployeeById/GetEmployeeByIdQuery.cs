using MediatR;

namespace Application.Features.Employees.Queries.GetEmployeeById
{
    public record GetEmployeeByIdQuery : IRequest<GetEmployeeByIdDto>
    {
        public int Id { get; set; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
