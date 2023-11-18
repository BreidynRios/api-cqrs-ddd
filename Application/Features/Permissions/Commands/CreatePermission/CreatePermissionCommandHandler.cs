using Application.Commons.Exceptions;
using Application.Commons.Utils;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.Common;
using Application.Interfaces.ServicesClients;
using Domain.Entities;
using MediatR;

namespace Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandHandler
        : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchServiceClient _elasticSearchServiceClient;

        public CreatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            IElasticSearchServiceClient elasticSearchServiceClient)
        {
            _unitOfWork = unitOfWork;
            _elasticSearchServiceClient = elasticSearchServiceClient;
        }

        public async Task<int> Handle(CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            await Validate(request, cancellationToken);
            var permission = AssignPermission(request);
            await _unitOfWork.PermissionRepository.AddAsync(permission, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await ElasticSearchCreateDocument(permission, cancellationToken);
            return permission.Id;
        }

        protected internal virtual async Task Validate(CreatePermissionCommand request,
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

        protected internal virtual Permission AssignPermission(CreatePermissionCommand request) 
        {
            return new()
            {
                EmployeeId = request.EmployeeId,
                PermissionTypeId = request.PermissionTypeId,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            };
        }

        protected internal virtual async Task ElasticSearchCreateDocument(
            Permission permission, CancellationToken cancellationToken)
        {
            await _elasticSearchServiceClient.CreateDocumentAsync(new PermissionParameter
            {
                OperationName = Constants.Request,
                PermissionId = permission.Id,
                EmployeeId = permission.EmployeeId,
                PermissionTypeId = permission.PermissionTypeId,
                CreatedBy = permission.CreatedBy,
                CreatedDate = permission.CreatedDate
            }, cancellationToken);
        }
    }
}
