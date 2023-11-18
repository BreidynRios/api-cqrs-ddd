using Application.Interfaces.ServicesClients;
using Confluent.Kafka;
using Infrastructure.Commons.Settings;
using Infrastructure.ServicesClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesClientsSettings>(configuration.GetSection("ServicesClients"));
            var settings = new ServicesClientsSettings();
            configuration.GetSection("ServicesClients").Bind(settings);

            services.AddElasticSearch();
            services.AddKafka();
        }

        private static void AddElasticSearch(this IServiceCollection services)
        {
            services.AddSingleton<IElasticSearchServiceClient, ElasticSearchServiceClient>();
            services.AddSingleton<IElasticClient>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<ServicesClientsSettings>>().Value;
                var uri = new Uri(settings.ElasticSearchServices.Host);
                var connectionSettings = new ConnectionSettings(uri)
                    .DefaultIndex(settings.ElasticSearchServices.DefaultIndex);

                return new ElasticClient(connectionSettings);
            });
        }

        private static void AddKafka(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaServiceClient, KafkaServiceClient>();
            services.AddSingleton(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<ServicesClientsSettings>>().Value;
                var producerConfig = new ProducerConfig 
                { 
                    BootstrapServers = settings.KafkaProducerServices.BootstrapServers 
                };
                return new ProducerBuilder<string, string>(producerConfig).Build();
            });
        }
    }
}
