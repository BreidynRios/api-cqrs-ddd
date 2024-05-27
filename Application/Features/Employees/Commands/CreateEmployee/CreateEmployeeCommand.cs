using MediatR;

namespace Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DocumentNumber { get; set; }
        public string Address { get; set; }
    }
}
