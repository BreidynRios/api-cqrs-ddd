using Application.Events.Interface;
using Application.Events.Messages;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Events.Subscriptions
{
    public class EmployeeCreatedSubscriptor : ISubscriber<EmployeeCreated>
    {
        private readonly ILogger<EmployeeCreatedSubscriptor> _logger;

        public EmployeeCreatedSubscriptor(ILogger<EmployeeCreatedSubscriptor> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(EventMessageContext<EmployeeCreated> message)
        {
            _logger.LogInformation("Employee created: {MESSAGE}", JsonSerializer.Serialize(message));
            await Task.CompletedTask;
        }
    }
}
