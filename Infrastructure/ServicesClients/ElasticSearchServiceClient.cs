using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Interfaces.ServicesClients;
using Infrastructure.Commons.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using System.Text.Json;

namespace Infrastructure.ServicesClients
{
    public class ElasticSearchServiceClient : IElasticSearchServiceClient
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchServices _elasticSearch;
        private readonly ILogger<ElasticSearchServiceClient> _logger;

        public ElasticSearchServiceClient(
            IElasticClient elasticClient,
            IOptions<ServicesClientsSettings> servicesClientsSettings,
            ILogger<ElasticSearchServiceClient> logger)
        {
            _elasticClient = elasticClient;
            _elasticSearch = servicesClientsSettings.Value.ElasticSearchServices;
            _logger = logger;
        }

        public async Task CreateDocumentAsync(PermissionParameter parameter, CancellationToken cancellationToken)
        {
            try
            {
                if (!_elasticSearch.Active) return;

                var response = await _elasticClient
                        .IndexDocumentAsync(parameter, cancellationToken);
                if (response.IsValid)
                    _logger.LogInformation("{message}", "ElasticSearch: The document was created. " +
                        $"Parameter: {JsonSerializer.Serialize(parameter)}");
                else
                    _logger.LogError("{message}", "ElasticSearch: The document wasn't created. " +
                        $"Parameter: {JsonSerializer.Serialize(parameter)}");
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", "ElasticSearch Exception: The document wasn't created. " +
                        $"Parameter: {JsonSerializer.Serialize(parameter)} " +
                        $"Error: {ex.Message}");
            }
        }
    }
}
