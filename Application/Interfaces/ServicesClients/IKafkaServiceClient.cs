using Application.DTOs.ServicesClients.Kafka;

namespace Application.Interfaces.ServicesClients
{
    public interface IKafkaServiceClient
    {
        Task ProduceAsync<T>(PermissionTopicParameter<T> message, CancellationToken cancellationToken);
    }
}
