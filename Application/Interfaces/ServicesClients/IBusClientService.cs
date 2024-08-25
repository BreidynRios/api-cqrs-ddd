using Application.Events.Interface;

namespace Application.Interfaces.ServicesClients
{
    public interface IBusClientService
    {
        Task<string> PublishMessageQueue<TMessage>(TMessage message, CancellationToken cancellationToken)
            where TMessage : IMessage;
    }
}
