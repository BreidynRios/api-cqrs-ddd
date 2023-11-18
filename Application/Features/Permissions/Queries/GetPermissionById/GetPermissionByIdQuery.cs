using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById
{
    public record GetPermissionByIdQuery : IRequest<GetPermissionByIdDto>
    {
        public int Id { get; set; }
        public GetPermissionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
