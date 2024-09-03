using Application.DTOs.Response;
using Application.Events.Messages;
using Application.Interfaces.ServicesClients;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler 
        : IRequestHandler<CreateEmployeeCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBusServiceClient _busClientService;

        public CreateEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            IEmployeeRepository employeeRepository,
            IBusServiceClient busClientService)
        {
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _busClientService = busClientService;
        }

        public async Task<Result<int>> Handle(CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = AssignEmployee(request);
            await _employeeRepository.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _busClientService.PublishMessageQueue(new EmployeeCreated
            {
                Id = employee.Id,
                Name = employee.Name
            }, cancellationToken);

            return Result<int>.Success(employee.Id);
        }

        protected virtual internal Employee AssignEmployee(CreateEmployeeCommand request)
        {
            return new()
            {
                Name = request.Name,
                Surname = request.Surname,
                DocumentNumber = request.DocumentNumber,
                Address = request.Address
            };
        }
    }
}
