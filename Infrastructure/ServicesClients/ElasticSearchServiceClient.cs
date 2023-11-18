using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.ServicesClients;
using Microsoft.Extensions.Logging;
using Nest;
using System.Text.Json;

namespace Infrastructure.ServicesClients
{
    public class ElasticSearchServiceClient : IElasticSearchServiceClient
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticSearchServiceClient> _logger;

        public ElasticSearchServiceClient(
            IElasticClient elasticClient,
            ILogger<ElasticSearchServiceClient> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task CreateDocumentAsync(PermissionParameter parameter, CancellationToken cancellationToken)
        {
            var response = await _elasticClient
                .IndexDocumentAsync(parameter, cancellationToken);
            if (response.IsValid)
                _logger.LogInformation($"ElasticSearch: The document was created. " +
                    $"Parameter: {JsonSerializer.Serialize(parameter)}");
            else
                _logger.LogError($"ElasticSearch: The document wasn't created. " +
                    $"Parameter: {JsonSerializer.Serialize(parameter)}");
        }
    }
}
