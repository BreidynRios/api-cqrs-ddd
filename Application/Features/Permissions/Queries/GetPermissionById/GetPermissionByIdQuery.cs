using Application.Commons.Behaviors.Interfaces;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public record GetPermissionByIdQuery : IRequest<GetPermissionByIdDto>, IRequestLogging
    {
        public int Id { get; set; }
        public GetPermissionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
