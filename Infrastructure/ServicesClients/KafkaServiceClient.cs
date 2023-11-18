using Application.DTOs.ServicesClients.Kafka;
using Application.Interfaces.ServicesClients;
using Confluent.Kafka;
using Infrastructure.Commons.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.ServicesClients
{
    public class KafkaServiceClient : IKafkaServiceClient
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaProducerServices _kafkaProducer;
        private readonly ILogger<KafkaServiceClient> _logger;

        public KafkaServiceClient(
            IProducer<string, string> producer,
            IOptions<ServicesClientsSettings> servicesClientsSettings,
            ILogger<KafkaServiceClient> logger)
        {
            _producer = producer;
            _kafkaProducer = servicesClientsSettings.Value.KafkaProducerServices;
            _logger = logger;
        }

        public async Task ProduceAsync<T>(PermissionTopicParameter<T> message, CancellationToken cancellationToken)
        {
            var topicMessage = new Message<string, string> 
            { 
                Value = JsonSerializer.Serialize(message) 
            };
            var result = await _producer.ProduceAsync(_kafkaProducer.Topic, topicMessage, cancellationToken);

            if (result.Status is PersistenceStatus.Persisted)
                _logger.LogInformation($"Kafka: Message persisted. Message: {topicMessage.Value}");
            else
                _logger.LogError($"Kafka: Message not persisted. Message: {topicMessage.Value}");
        }
    }
}
