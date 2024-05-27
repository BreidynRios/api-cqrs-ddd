using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public class GetAllPermissionTypesQueryHandler 
        : IRequestHandler<GetAllPermissionTypesQuery, IEnumerable<GetAllPermissionTypesDto>>
    {
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPermissionTypesQueryHandler(
            IPermissionTypeRepository permissionTypeRepository,
            IMapper mapper)
        {
            _permissionTypeRepository = permissionTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllPermissionTypesDto>> Handle(
            GetAllPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            var permissionTypes = await _permissionTypeRepository
                .GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<GetAllPermissionTypesDto>>(permissionTypes);
        }
    }
}
