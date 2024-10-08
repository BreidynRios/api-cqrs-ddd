﻿using Application.Commons.Utils;
using Application.DTOs.Response;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.ServicesClients;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandHandler
        : IRequestHandler<CreatePermissionCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchServiceClient _elasticSearchServiceClient;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public CreatePermissionCommandHandler(
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

        public async Task<Result<int>> Handle(CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var error = await Validate(request, cancellationToken);
            if (!string.IsNullOrEmpty(error))
                return Result<int>.Failure(error);

            var permission = AssignPermission(request);
            await _permissionRepository.AddAsync(permission, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await ElasticSearchCreateDocument(permission, cancellationToken);

            return Result<int>.Success(permission.Id);
        }

        protected internal virtual async Task<string> Validate(CreatePermissionCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository
                .GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee is null)
                return "Employee wasn't found";

            var permissionType = await _permissionTypeRepository
                .GetByIdAsync(request.PermissionTypeId, cancellationToken);
            if (permissionType is null)
                return "Permission type wasn't found";

            return string.Empty;
        }

        protected internal virtual Permission AssignPermission(CreatePermissionCommand request) 
        {
            return new()
            {
                EmployeeId = request.EmployeeId,
                PermissionTypeId = request.PermissionTypeId
            };
        }

        protected internal virtual async Task ElasticSearchCreateDocument(
            Permission permission, CancellationToken cancellationToken)
        {
            await _elasticSearchServiceClient.CreateDocumentAsync(new PermissionParameter
            {
                OperationName = GeneralConstants.REQUEST,
                PermissionId = permission.Id,
                EmployeeId = permission.EmployeeId,
                PermissionTypeId = permission.PermissionTypeId,
                CreatedBy = permission.CreatedBy,
                CreatedDateOnUtc = permission.CreatedDateOnUtc
            }, cancellationToken);
        }
    }
}
