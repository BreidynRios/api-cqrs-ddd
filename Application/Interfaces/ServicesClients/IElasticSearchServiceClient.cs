using Application.DTOs.ServicesClients.ElasticSearch;

namespace Application.Interfaces.ServicesClients
{
    public interface IElasticSearchServiceClient
    {
        Task CreateDocumentAsync(PermissionParameter parameter, CancellationToken cancellationToken);
    }
}
