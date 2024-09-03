using Application.DTOs.Response;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public class GetPermissionTypeByIdQueryHandler 
        : IRequestHandler<GetPermissionTypeByIdQuery, Result<GetPermissionTypeByIdDto>>
    {
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        private readonly IMapper _mapper;

        public GetPermissionTypeByIdQueryHandler(
            IPermissionTypeRepository permissionTypeRepository,
            IMapper mapper)
        {
            _permissionTypeRepository = permissionTypeRepository;
            _mapper = mapper;
        }

        public async Task<Result<GetPermissionTypeByIdDto>> Handle(
            GetPermissionTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var permissionType = await _permissionTypeRepository
                .GetByIdAsync(request.Id, cancellationToken);
            if (permissionType is null)
                return Result<GetPermissionTypeByIdDto>.Failure("Permission type wasn't found");

            return Result<GetPermissionTypeByIdDto>
                .Success(_mapper.Map<GetPermissionTypeByIdDto>(permissionType));
        }
    }
}
