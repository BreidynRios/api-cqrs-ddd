using Application.Commons.Exceptions;
using Application.Interfaces.Common;
using AutoMapper;
using MediatR;

namespace Application.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler
        : IRequestHandler<GetEmployeeByIdQuery, GetEmployeeByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetEmployeeByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetEmployeeByIdDto> Handle(
            GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork
                .EmployeeRepository
                .GetEmployeeWithPermissionsAsync(request.Id, cancellationToken);
            if (employee is null)
                throw new NotFoundException(nameof(employee), request.Id);

            return _mapper.Map<GetEmployeeByIdDto>(employee);
        }
    }
}
