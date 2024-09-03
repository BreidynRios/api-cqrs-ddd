using Application.Commons.Behaviors.Interfaces;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Permissions.Commands.CreatePermission
{
    public record CreatePermissionCommand : IRequest<Result<int>>, IRequestLogging
    {
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
