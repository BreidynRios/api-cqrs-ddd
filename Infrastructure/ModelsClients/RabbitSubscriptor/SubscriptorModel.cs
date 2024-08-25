namespace Infrastructure.ModelsClients.RabbitSubscriptor
{
    public class SubscriptorModel
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public List<MessageModel> Messages { get; set; }
    }

    public class MessageModel
    {
        public string Name { get; set; }

        public Type Type { get; set; }
    }
}
