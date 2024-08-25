using MediatR;

namespace Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommand : IRequest
    {
        public int PermissionId { get; set; }
        public DeletePermissionCommand(int permissionId)
        {
            PermissionId = permissionId;
        }
    }
}
