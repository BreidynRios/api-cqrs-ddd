namespace Application.Events.Interface
{
    public interface ISubscriber<TMessage> where TMessage : IMessage
    {
        Task HandleAsync(EventMessageContext<TMessage> message);
    }
}
