using Application.Commons.Behaviors.Interfaces;
using MediatR;

namespace Application.Features.Permissions.Commands.CreatePermission
{
    public record CreatePermissionCommand : IRequest<int>, IRequestLogging
    {
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
