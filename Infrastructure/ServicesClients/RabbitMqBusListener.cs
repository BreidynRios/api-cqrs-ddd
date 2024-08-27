using Application.Events;
using Application.Events.Interface;
using Infrastructure.Commons.Settings;
using Infrastructure.ModelsClients.RabbitSubscriptor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Infrastructure.ServicesClients
{
    public class RabbitMqBusListener : IHostedService
    {
        private readonly ILogger<RabbitMqBusListener> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;
        private readonly ICollection<SubscriptorModel> RegisteredSubscriptors = [];
        private readonly IEnumerable<Type> _subscriptors;

        public RabbitMqBusListener(
            IOptions<RabbitMqSettings> options,
            ILogger<RabbitMqBusListener> logger,
            IServiceProvider serviceProvider,
            IEnumerable<Type> subscribtors)
        {
            var configuration = options.Value;
            _connectionFactory = new ConnectionFactory
            {
                HostName = configuration.Host,
                Port = configuration.Port,
                UserName = configuration.Username,
                Password = configuration.Password,
                VirtualHost = configuration.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10.0),
                RequestedHeartbeat = TimeSpan.FromMinutes(5.0),
                DispatchConsumersAsync = true
            };
            _logger = logger;
            _serviceProvider = serviceProvider;
            _subscriptors = subscribtors;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection?.Close();
            _channel?.Close();
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ExecuteAndRetryAction(RefreshConnectionChannel);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ProcessMessagesForSubscriberAsync;

            foreach (var subscriptor in _subscriptors)
            {
                var queueName = subscriptor.FullName;
                _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, null);

                var requestSubscriptor = subscriptor.GetInterfaces()
                    .Where(i => i.GetGenericTypeDefinition() == typeof(ISubscriber<>))
                    .Select(subscriberInterface => subscriberInterface.GetGenericArguments()[0]);

                foreach (var exchangeName in requestSubscriptor.Select(rs => rs.Name))
                {
                    _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);
                    _channel.QueueBind(queueName, exchangeName, exchangeName, null);
                }

                _channel.BasicConsume(queueName, false, consumer);
                AssignSubscriptor(subscriptor, requestSubscriptor);
            }
            return Task.CompletedTask;
        }

        protected internal void AssignSubscriptor(Type subscriptor, IEnumerable<Type> requestSubscriptor)
        {
            RegisteredSubscriptors.Add(new SubscriptorModel
            {
                Type = subscriptor,
                Name = subscriptor.FullName,
                Messages = requestSubscriptor.Select((Type message) => new MessageModel
                {
                    Type = message,
                    Name = message.Name
                }).ToList()
            });
        }

        protected internal async Task ProcessMessagesForSubscriberAsync(
            object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Mensaje recibido {MESSAGE}: ", message);

            var subscriptor = RegisteredSubscriptors.FirstOrDefault(rs => rs.Messages
                .Exists(m => m.Name == eventArgs.RoutingKey));
            if (subscriptor is null)
            {
                _logger.LogError("No existe el subscriptor {NAME} y contenido {MESSAGE}",
                    eventArgs.RoutingKey, message);
                return;
            }

            var requestSubscriptor = subscriptor.Messages.First(m => m.Name == eventArgs.RoutingKey);
            var request = JsonSerializer.Deserialize(message, requestSubscriptor.Type, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (request is null)
            {
                _logger.LogError("El mensaje a procesar no tiene la información correcta");
                return;
            }

            var subscriptorType = subscriptor.Type;
            var handlerInstance = _serviceProvider.GetService(subscriptorType);
            if (handlerInstance is null)
            {
                _logger.LogError("No se recuperó el subscriptor {TYPE}", subscriptorType);
                return;
            }

            var methodInfo = subscriptorType.GetMethod("HandleAsync");
            if (methodInfo is null)
            {
                _logger.LogError("El subscriptor {TYPE} no tiene el método 'HandleAsync'", subscriptorType);
                return;
            }

            var messageInstance = CreateMessageContext(request, eventArgs.BasicProperties.Headers);
            await Task.FromResult(methodInfo.Invoke(handlerInstance, [messageInstance]));
            _channel.BasicAck(eventArgs.DeliveryTag, false);
        }

        protected internal object? CreateMessageContext(object request, IDictionary<string, object> headers)
        {
            var instanciaKey = headers is not null && headers.TryGetValue("key", out object? key)
                    ? Encoding.UTF8.GetString((byte[])key)
                    : string.Empty;
            var instanciaApp = headers is not null && headers.TryGetValue("application", out object? app)
                ? Encoding.UTF8.GetString((byte[])app)
                : string.Empty;

            var typeFromHandle = typeof(EventMessageContext<>);
            var typeArguments = new Type[1] { request.GetType() };

            return Activator.CreateInstance(typeFromHandle.MakeGenericType(typeArguments),
                request, instanciaKey, instanciaApp);
        }

        protected internal void ExecuteAndRetryAction(Action reconectAction)
        {
            Policy.Handle<Exception>().WaitAndRetry(
            [
                TimeSpan.FromSeconds(1.0),
                TimeSpan.FromSeconds(3.0)
            ], delegate (Exception exception, TimeSpan _, Context _)
            {
                _logger.LogError(exception, "[Policy] Ocurrió un error con Rabbit.");
            }).Execute(reconectAction);
        }

        protected internal void RefreshConnectionChannel()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection();
                _connection.ConnectionShutdown += ConnectionShutdownEvent;
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _connection.CreateModel();
            }
        }

        protected internal void ConnectionShutdownEvent(object? sender, ShutdownEventArgs e)
        {
            _logger.LogError("ConnectionShutdown_Event: Se perdió la conexión de Rabbit");
            ExecuteAndRetryAction(RefreshConnectionChannel);
        }
    }
}
