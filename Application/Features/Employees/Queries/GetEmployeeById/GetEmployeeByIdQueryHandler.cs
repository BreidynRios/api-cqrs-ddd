using Application.DTOs.Response;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler
        : IRequestHandler<GetEmployeeByIdQuery, Result<GetEmployeeByIdDto>>
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

        public async Task<Result<GetEmployeeByIdDto>> Handle(
            GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository
                .GetEmployeeWithPermissionsAsync(request.Id, cancellationToken);
            if (employee is null)
                return Result<GetEmployeeByIdDto>.Failure("Employee wasn't found");

            return Result<GetEmployeeByIdDto>.Success(_mapper.Map<GetEmployeeByIdDto>(employee));
        }
    }
}
