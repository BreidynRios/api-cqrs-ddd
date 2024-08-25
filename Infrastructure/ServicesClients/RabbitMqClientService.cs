﻿using Application.Events.Interface;
using Application.Interfaces.ServicesClients;
using Infrastructure.Commons.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.ServicesClients
{
    public class RabbitMqClientService : IBusClientService
    {
        private readonly ILogger<RabbitMqClientService> _logger;
        private readonly RabbitMqSettings _configuration;
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMqClientService(
            IOptions<RabbitMqSettings> options,
            ILogger<RabbitMqClientService> logger)
        {
            _configuration = options.Value;
            _connectionFactory = new ConnectionFactory
            {
                HostName = _configuration.Host,
                Port = _configuration.Port,
                UserName = _configuration.Username,
                Password = _configuration.Password,
                VirtualHost = _configuration.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10.0),
                RequestedHeartbeat = TimeSpan.FromMinutes(5.0)
            };
            _logger = logger;
        }

        public async Task<string> PublishMessageQueue<TMessage>(TMessage message,
            CancellationToken cancellationToken) where TMessage : IMessage
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            var basicProperties = channel.CreateBasicProperties();

            var messageKey = Guid.NewGuid().ToString();
            basicProperties.Headers = new Dictionary<string, object>
            {
                { "key", messageKey },
                { "application", _configuration.ApplicationKey }
            };

            var exchangeName = message.GetType().Name;
            var messageContent = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageContent);
            channel.BasicPublish(exchangeName, exchangeName, basicProperties, body);

            _logger.LogInformation("Ha sido publicado un Mensaje con Key: '{KEY}', Nombre: '{NAME}' " +
                "y Contenido: '{MESSAGE}'.", messageKey, exchangeName, messageContent);

            return await Task.FromResult(messageKey);
        }
    }
}