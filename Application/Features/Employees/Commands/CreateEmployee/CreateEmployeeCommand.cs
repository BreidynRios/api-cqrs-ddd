using Application.Commons.Behaviors.Interfaces;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<Result<int>>, IRequestValidation
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
    }
}
