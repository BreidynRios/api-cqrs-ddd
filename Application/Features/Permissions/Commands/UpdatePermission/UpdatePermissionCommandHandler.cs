using Application.Commons.Exceptions;
using Application.Commons.Utils;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.Common;
using Application.Interfaces.ServicesClients;
using Domain.Entities;
using MediatR;

namespace Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler
       : IRequestHandler<UpdatePermissionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchServiceClient _elasticSearchServiceClient;

        public UpdatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            IElasticSearchServiceClient elasticSearchServiceClient)
        {
            _unitOfWork = unitOfWork;
            _elasticSearchServiceClient = elasticSearchServiceClient;
        }

        public async Task Handle(UpdatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            await Validate(request, cancellationToken);
            var permission = await _unitOfWork
                .PermissionRepository
                .GetByIdAsync(request.Id, cancellationToken);

            AssignPermission(permission, request);
            await _unitOfWork.PermissionRepository.UpdateAsync(permission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await ElasticSearchCreateDocument(permission, cancellationToken);
        }

        protected internal virtual async Task Validate(UpdatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork
                .EmployeeRepository
                .GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee is null)
                throw new NotFoundException(nameof(employee), request.EmployeeId);

            var permissionType = await _unitOfWork
                .PermissionTypeRepository
                .GetByIdAsync(request.PermissionTypeId, cancellationToken);
            if (permissionType is null)
                throw new NotFoundException(nameof(permissionType), request.PermissionTypeId);
        }

        protected internal virtual void AssignPermission(
            Permission permission, UpdatePermissionCommand request)
        {
            permission.EmployeeId = request.EmployeeId;
            permission.PermissionTypeId = request.PermissionTypeId;
            permission.UpdatedBy = 1;
            permission.UpdatedDate = DateTime.UtcNow;
        }

        protected internal virtual async Task ElasticSearchCreateDocument(
            Permission permission, CancellationToken cancellationToken)
        {
            await _elasticSearchServiceClient.CreateDocumentAsync(new PermissionParameter
            {
                OperationName = Constants.Modify,
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
