using Application.Commons.Exceptions;
using Application.Commons.Utils;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.ServicesClients;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler
       : IRequestHandler<UpdatePermissionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchServiceClient _elasticSearchServiceClient;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public UpdatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            IElasticSearchServiceClient elasticSearchServiceClient,
            IPermissionRepository permissionRepository,
            IEmployeeRepository employeeRepository,
            IPermissionTypeRepository permissionTypeRepository)
        {
            _unitOfWork = unitOfWork;
            _elasticSearchServiceClient = elasticSearchServiceClient;
            _permissionRepository = permissionRepository;
            _employeeRepository = employeeRepository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        public async Task Handle(UpdatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            await Validate(request, cancellationToken);
            var permission = await _permissionRepository
                .GetByIdAsync(request.Id, cancellationToken);

            AssignPermission(permission, request);
            _permissionRepository.Update(permission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await ElasticSearchCreateDocument(permission, cancellationToken);
        }

        protected internal virtual async Task Validate(UpdatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository
                .GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee is null)
                throw new NotFoundException("Employee wasn't found");

            var permissionType = await _permissionTypeRepository
                .GetByIdAsync(request.PermissionTypeId, cancellationToken);
            if (permissionType is null)
                throw new NotFoundException("Permission type wasn't found");
        }

        protected internal virtual void AssignPermission(
            Permission permission, UpdatePermissionCommand request)
        {
            permission.EmployeeId = request.EmployeeId;
            permission.PermissionTypeId = request.PermissionTypeId;
            permission.UpdatedBy = 1;
        }

        protected internal virtual async Task ElasticSearchCreateDocument(
            Permission permission, CancellationToken cancellationToken)
        {
            await _elasticSearchServiceClient.CreateDocumentAsync(new PermissionParameter
            {
                OperationName = GeneralConstants.MODIFY,
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
