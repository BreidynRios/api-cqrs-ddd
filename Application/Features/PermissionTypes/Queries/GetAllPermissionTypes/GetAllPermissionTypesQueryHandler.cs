using Application.Interfaces.Common;
using AutoMapper;
using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public class GetAllPermissionTypesQueryHandler 
        : IRequestHandler<GetAllPermissionTypesQuery, IEnumerable<GetAllPermissionTypesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllPermissionTypesQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllPermissionTypesDto>> Handle(
            GetAllPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            var permissionTypes = await _unitOfWork
                .PermissionTypeRepository
                .GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<GetAllPermissionTypesDto>>(permissionTypes);
        }
    }
}
