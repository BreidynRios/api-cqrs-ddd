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
            _logger.LogInformation($"Method: {request}, Start Date: {DateTime.UtcNow}");
            
            var response = await next();
            
            _logger.LogInformation($"Method: {request}, End Date: {DateTime.UtcNow}");
            
            return response;
        }
    }
}
