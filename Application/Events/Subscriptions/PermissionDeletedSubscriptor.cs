using Application.Events.Interface;
using Application.Events.Messages;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Events.Subscriptions
{
    public class PermissionDeletedSubscriptor : ISubscriber<PermissionDeleted>
    {
        private readonly ILogger<PermissionDeletedSubscriptor> _logger;

        public PermissionDeletedSubscriptor(ILogger<PermissionDeletedSubscriptor> logger)
        {
            _logger = logger;
        }
        public async Task HandleAsync(EventMessageContext<PermissionDeleted> message)
        {
            _logger.LogInformation("Permission deleted: {MESSAGE}", JsonSerializer.Serialize(message));
            await Task.CompletedTask;
        }
    }
}
