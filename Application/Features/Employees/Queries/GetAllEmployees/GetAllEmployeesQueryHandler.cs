using Application.Interfaces.Common;
using AutoMapper;
using MediatR;

namespace Application.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQueryHandler 
        : IRequestHandler<GetAllEmployeesQuery, IEnumerable<GetAllEmployeesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllEmployeesQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllEmployeesDto>> Handle(
            GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork
                .EmployeeRepository
                .GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<GetAllEmployeesDto>>(employees);
        }
    }
}
