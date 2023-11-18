using Application.Commons.Exceptions;
using Application.Commons.Utils;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.Common;
using Application.Interfaces.ServicesClients;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQueryHandler
        : IRequestHandler<GetPermissionByIdQuery, GetPermissionByIdDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IElasticSearchServiceClient _elasticSearchServiceClient;

        public GetPermissionByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IElasticSearchServiceClient elasticSearchServiceClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _elasticSearchServiceClient = elasticSearchServiceClient;
        }

        public async Task<GetPermissionByIdDto> Handle(
            GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork
                .PermissionRepository
                .GetPermissionWithEmployeeTypeAsync(request.Id, cancellationToken);
            if (permission is null)
                throw new NotFoundException(nameof(permission), request.Id);

            await ElasticSearchCreateDocument(permission, cancellationToken);
            return _mapper.Map<GetPermissionByIdDto>(permission);
        }

        protected internal virtual async Task ElasticSearchCreateDocument(
            Permission permission, CancellationToken cancellationToken)
        {
            await _elasticSearchServiceClient.CreateDocumentAsync(new PermissionParameter
            {
                OperationName = Constants.Get,
                PermissionId = permission.Id,
                EmployeeId = permission.EmployeeId,
                PermissionTypeId = permission.PermissionTypeId,
                CreatedBy = permission.CreatedBy,
                CreatedDate = permission.CreatedDate,
                UpdatedBy = permission.UpdatedBy,
                UpdatedDate = permission.UpdatedDate,
            }, cancellationToken);
        }
    }
}
