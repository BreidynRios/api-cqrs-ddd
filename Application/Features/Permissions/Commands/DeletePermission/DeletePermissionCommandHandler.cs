﻿using Application.Commons.Exceptions;
using Application.Events.Messages;
using Application.Interfaces.ServicesClients;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IBusClientService _busClientService;

        public DeletePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            IPermissionRepository permissionRepository,
            IBusClientService busClientService)
        {
            _unitOfWork = unitOfWork;
            _permissionRepository = permissionRepository;
            _busClientService = busClientService;
        }

        public async Task Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository
                .GetByIdAsync(request.PermissionId, cancellationToken);
            if (permission is null)
                throw new NotFoundException("Permission wasn't found");

            _permissionRepository.Delete(permission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _busClientService.PublishMessageQueue(new PermissionDeleted
            {
                EmployeeId = permission.EmployeeId,
                PermissionTypeId = permission.PermissionTypeId
            }, cancellationToken);
        }
    }
}
