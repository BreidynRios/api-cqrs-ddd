using Application.Commons.Behaviors.Interfaces;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public record GetPermissionByIdQuery : IRequest<Result<GetPermissionByIdDto>>, IRequestLogging
    {
        public int Id { get; set; }
        public GetPermissionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
