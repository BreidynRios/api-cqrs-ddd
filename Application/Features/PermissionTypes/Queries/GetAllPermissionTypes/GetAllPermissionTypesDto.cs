using Application.Commons.Mappings;
using Domain.Entities;

namespace Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public class GetAllPermissionTypesDto : IMapFrom<PermissionType>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
