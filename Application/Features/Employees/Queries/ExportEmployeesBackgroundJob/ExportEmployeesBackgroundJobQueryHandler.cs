using Application.Interfaces.ServicesClients;
using Application.SignalRHub;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Employees.Queries.ExportEmployeesBackgroundJob
{
    public class ExportEmployeesBackgroundJobQueryHandler 
        : IRequestHandler<ExportEmployeesBackgroundJobQuery, string>
    {
        protected readonly IBackgroundJobServiceClient _backgroundJobService;
        protected IHubContext<ApplicationHub, IApplicationHubClient> _applicationHubContext;
        private readonly ILogger<ExportEmployeesBackgroundJobQueryHandler> _logger;

        public ExportEmployeesBackgroundJobQueryHandler(
            IBackgroundJobServiceClient backgroundJobService,
            IHubContext<ApplicationHub, IApplicationHubClient> applicationHubContext,
            ILogger<ExportEmployeesBackgroundJobQueryHandler> logger)
        {
            _backgroundJobService = backgroundJobService;
            _applicationHubContext = applicationHubContext;
            _logger = logger;
        }

        public async Task<string> Handle(ExportEmployeesBackgroundJobQuery request,
            CancellationToken cancellationToken)
        {
            var taskId = Guid.NewGuid();
            _backgroundJobService.EnqueueJob(() => ExecuteBackgroundJob(taskId, request));
            return await Task.FromResult(taskId.ToString());
        }

        protected internal virtual void ExecuteBackgroundJob(
            Guid taskId, ExportEmployeesBackgroundJobQuery request)
        {
            try
            {
                var cancellationTokenSource = _backgroundJobService.GetJob(taskId);
                _logger.LogInformation("{MESSAGE}", GetMessage("Proceso de backgroundjob iniciado", taskId));

                _applicationHubContext.Clients.All.NewJob($"Exportación Iniciada {taskId}");
                Task.Delay(5000).Wait();
                
                cancellationTokenSource?.Token.ThrowIfCancellationRequested();

                Task.Delay(5000).Wait();
                _applicationHubContext.Clients.All.FinishedJob($"Exportación Finalizada {taskId}");
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "{MESSAGE}", GetMessage("El proceso de backgroundjob fue cancelado", taskId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MESSAGE}", GetMessage("Ocurrió un error procesando el backgroundjob", taskId));
            }
            finally
            {
                _logger.LogInformation("{MESSAGE}", GetMessage("Proceso de backgroundjob finalizado", taskId));
            }
        }

        protected internal virtual string GetMessage(string message, Guid taskId)
        {
            return $"{message}. Método {GetType().FullName}, TaskId: {taskId}, " +
                $"Fecha: {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}";
        }
    }
}
