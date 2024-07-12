using Application.Commons.Behaviors.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commons.Behaviors
{
    public class RequestLoggingPipelineBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequestLogging
    {
        private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public RequestLoggingPipelineBehavior(
            ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var messageStart = $"Method: {request}, Start Date: {DateTime.UtcNow}";
            _logger.LogInformation("{Message}", messageStart);
            
            var response = await next();

            var messageEnd = $"Method: {request}, End Date: {DateTime.UtcNow}";
            _logger.LogInformation("{Message}", messageEnd);
            
            return response;
        }
    }
}
