using Application.Commons.Exceptions;
using Application.Interfaces.Common;
using AutoMapper;
using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public class GetPermissionTypeByIdQueryHandler 
        : IRequestHandler<GetPermissionTypeByIdQuery, GetPermissionTypeByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetPermissionTypeByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetPermissionTypeByIdDto> Handle(
            GetPermissionTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork
                .PermissionTypeRepository
                .GetByIdAsync(request.Id, cancellationToken);
            if (permissionType is null)
                throw new NotFoundException(nameof(permissionType), request.Id);

            return _mapper.Map<GetPermissionTypeByIdDto>(permissionType);
        }
    }
}
