using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public record GetAllPermissionTypesQuery : IRequest<IEnumerable<GetAllPermissionTypesDto>>;
}
