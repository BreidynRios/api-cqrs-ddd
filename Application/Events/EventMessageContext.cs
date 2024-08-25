using Application.Events.Interface;

namespace Application.Events
{
    public class EventMessageContext<T> where T : IMessage
    {
        public T Message { get; set; }

        public string Key { get; set; }

        public string Application { get; set; }

        public EventMessageContext(T message, string key, string application)
        {
            Message = message;
            Key = key;
            Application = application;
        }

        public EventMessageContext()
        {
        }
    }
}
