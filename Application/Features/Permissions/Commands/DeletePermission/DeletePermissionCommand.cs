using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommand : IRequest<Result>
    {
        public int PermissionId { get; set; }
        public DeletePermissionCommand(int permissionId)
        {
            PermissionId = permissionId;
        }
    }
}
