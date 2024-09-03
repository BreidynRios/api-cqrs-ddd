using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Employees.Queries.GetEmployeeById
{
    public record GetEmployeeByIdQuery : IRequest<Result<GetEmployeeByIdDto>>
    {
        public int Id { get; set; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
