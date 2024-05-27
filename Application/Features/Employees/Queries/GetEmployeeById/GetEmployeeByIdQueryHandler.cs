using Application.Commons.Exceptions;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler
        : IRequestHandler<GetEmployeeByIdQuery, GetEmployeeByIdDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeByIdDto> Handle(
            GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository
                .GetEmployeeWithPermissionsAsync(request.Id, cancellationToken);
            if (employee is null)
                throw new NotFoundException("Employee wasn't found");

            return _mapper.Map<GetEmployeeByIdDto>(employee);
        }
    }
}
