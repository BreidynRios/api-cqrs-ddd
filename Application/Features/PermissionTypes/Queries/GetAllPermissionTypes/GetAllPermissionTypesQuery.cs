using Application.Commons.Behaviors.Interfaces;
using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public record GetAllPermissionTypesQuery : 
        IRequest<IEnumerable<GetAllPermissionTypesDto>>, IDataCache
    {
        public string CacheKey => "permission-types";

        public TimeSpan Expiration => TimeSpan.FromHours(1);
    }
}
