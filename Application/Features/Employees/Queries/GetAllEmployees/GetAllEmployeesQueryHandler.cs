using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQueryHandler 
        : IRequestHandler<GetAllEmployeesQuery, IEnumerable<GetAllEmployeesDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllEmployeesDto>> Handle(
            GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository
                .GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<GetAllEmployeesDto>>(employees);
        }
    }
}
