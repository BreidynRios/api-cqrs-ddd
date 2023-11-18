using Application.Commons.Mappings;
using Domain.Entities;

namespace Application.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public class GetPermissionTypeByIdDto : IMapFrom<PermissionType>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
