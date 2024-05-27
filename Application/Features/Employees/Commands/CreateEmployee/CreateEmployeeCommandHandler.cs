using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler 
        : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            IEmployeeRepository employeeRepository)
        {
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = AssignEmployee(request);
            await _employeeRepository.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return employee.Id;
        }

        protected virtual internal Employee AssignEmployee(CreateEmployeeCommand request)
        {
            return new()
            {
                Name = request.Name,
                Surname = request.Surname,
                DocumentNumber = request.DocumentNumber,
                Address = request.Address,
                CreatedBy = 1
            };
        }
    }
}
