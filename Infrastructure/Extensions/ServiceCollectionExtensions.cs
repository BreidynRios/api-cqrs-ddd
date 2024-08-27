using Application.Events.Subscriptions;
using Application.Interfaces.ServicesClients;
using Confluent.Kafka;
using Infrastructure.Commons.Settings;
using Infrastructure.ServicesClients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nest;
using StackExchange.Redis;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddExternalServicesClients(configuration);
            services.AddElasticSearch();
            services.AddKafka();
            services.AddRedis(configuration);
            services.AddRabbitMq(configuration, environment);
            services.AddBackgroundJob();
        }

        private static void AddExternalServicesClients(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ServicesClientsSettings>(configuration.GetSection("ServicesClients"));
            var settings = new ServicesClientsSettings();
            configuration.GetSection("ServicesClients").Bind(settings);
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

        private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = ConfigurationOptions.Parse($"{configuration["Redis:Host"]}, " +
                $"password={configuration["Redis:Password"]}, allowAdmin=true");

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisConfig;
            });
        }

        private static void AddRabbitMq(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("MessageBroker"));
            services.AddSingleton<IBusServiceClient, RabbitMqServiceClient>();

            if (environment.IsDevelopment()) return;

            var subscriptors = new Type[] { 
                typeof(EmployeeCreatedSubscriptor),
                typeof(PermissionDeletedSubscriptor)
            };

            services.AddSingleton<IHostedService, RabbitMqBusListener>((IServiceProvider provider) =>
                ActivatorUtilities.CreateInstance<RabbitMqBusListener>(provider, [subscriptors.Cast<object>()]));

            foreach (var subscriptor in subscriptors)
            {
                services.AddTransient(subscriptor);
            }
        }

        private static void AddBackgroundJob(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundJobServiceClient, BackgroundJobServiceClient>();
        }
    }
}
