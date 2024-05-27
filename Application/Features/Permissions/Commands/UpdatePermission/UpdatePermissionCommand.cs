using Application.Commons.Behaviors.Interfaces;
using MediatR;

namespace Application.Features.Permissions.Commands.UpdatePermission
{
    public record UpdatePermissionCommand : IRequest, IRequestLogging
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
