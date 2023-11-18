namespace Application.DTOs.ServicesClients.Kafka
{
    public class PermissionTopicParameter<T>
    {
        public Guid Id { get; set; }
        public string OperationName { get; set; }
        public T Data { get; set; }
        public PermissionTopicParameter(string operationName, T data)
        {
            Id = Guid.NewGuid();
            OperationName = operationName;
            Data = data;
        }
    }
}
