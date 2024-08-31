using Application.Commons.Exceptions;
using Application.Commons.Utils;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.ServicesClients;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQueryHandler
        : IRequestHandler<GetPermissionByIdQuery, GetPermissionByIdDto>
    {
        private readonly IMapper _mapper;
        private readonly IElasticSearchServiceClient _elasticSearchServiceClient;
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByIdQueryHandler(
            IMapper mapper,
            IElasticSearchServiceClient elasticSearchServiceClient,
            IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _elasticSearchServiceClient = elasticSearchServiceClient;
            _permissionRepository = permissionRepository;
        }

        public async Task<GetPermissionByIdDto> Handle(
            GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository
                .GetPermissionWithEmployeeTypeAsync(request.Id, cancellationToken);
            if (permission is null)
                throw new NotFoundException("Permission wasn't found");

            await ElasticSearchCreateDocument(permission, cancellationToken);
            return _mapper.Map<GetPermissionByIdDto>(permission);
        }

        protected internal virtual async Task ElasticSearchCreateDocument(
            Permission permission, CancellationToken cancellationToken)
        {
            await _elasticSearchServiceClient.CreateDocumentAsync(new PermissionParameter
            {
                OperationName = GeneralConstants.GET,
                PermissionId = permission.Id,
                EmployeeId = permission.EmployeeId,
                PermissionTypeId = permission.PermissionTypeId,
                CreatedBy = permission.CreatedBy,
                CreatedDateOnUtc = permission.CreatedDateOnUtc,
                UpdatedBy = permission.UpdatedBy,
                UpdatedDateOnUtc = permission.UpdatedDateOnUtc,
            }, cancellationToken);
        }
    }
}
