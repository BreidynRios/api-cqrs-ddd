using Application.Commons.Behaviors.Interfaces;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Permissions.Commands.UpdatePermission
{
    public record UpdatePermissionCommand : IRequest<Result>, IRequestLogging
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
