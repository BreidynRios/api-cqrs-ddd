namespace Infrastructure.Commons.Settings
{
    public class ServicesClientsSettings
    {
        public ElasticSearchServices ElasticSearchServices { get; set; }
        public KafkaProducerServices KafkaProducerServices { get; set; }
    }

    public class ElasticSearchServices
    {
        public string Host { get; set; }
        public string DefaultIndex { get; set; }
    }

    public class KafkaProducerServices
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
    }
}
