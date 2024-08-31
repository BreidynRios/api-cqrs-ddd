using Application.Commons.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private const string InternalError = "Internal Error";
        private const string InternalErrorMessage = "An internal error has occurred";

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var resultError = SetErrorDetail(exception);

            _logger.LogError(exception, "Error: {Message}", resultError.Detail);

            await httpContext.Response.WriteAsJsonAsync(resultError, cancellationToken);

            return true;
        }

        private static ProblemDetails SetErrorDetail(Exception exception)
        {
            var baseException = exception as BaseException;
            return new()
            {
                Status = baseException?.StatusCode ?? StatusCodes.Status500InternalServerError,
                Title = baseException?.Title ?? InternalError,
                Detail = baseException?.Message ?? $"{InternalErrorMessage}. Internal Code: {DateTime.UtcNow.Ticks}"
            };
        }
    }
}
