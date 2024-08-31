using Application.Events.Interface;

namespace Application.Interfaces.ServicesClients
{
    public interface IBusServiceClient
    {
        Task<string?> PublishMessageQueue<TMessage>(TMessage message, CancellationToken cancellationToken)
            where TMessage : IMessage;
    }
}
