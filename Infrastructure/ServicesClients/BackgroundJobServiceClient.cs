using Application.Interfaces.ServicesClients;
using Infrastructure.ModelsClients.BackgroundJob;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.ServicesClients
{
    public class BackgroundJobServiceClient : IBackgroundJobServiceClient
    {
        private readonly ILogger<BackgroundJobServiceClient> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<TaskTokenCacheModel> _taskTokensCache = [];

        public BackgroundJobServiceClient(
            ILogger<BackgroundJobServiceClient> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void EnqueueJob(Expression<Action> methodCall)
        {
            var call = methodCall.Body as MethodCallExpression 
                ?? throw new Application.Commons.Exceptions.ApplicationException(
                    "El tipo de expresión debe ser MethodCallExpression");

            var methodInfo = call.Method;
            var argsValues = call.Arguments
                .Select(a =>
                {
                    var arg = Expression.Convert(a, typeof(object));
                    return Expression.Lambda<Func<object>>(arg, null).Compile()();
                })
                .ToArray();

            var taskId = (Guid)argsValues[0];
            var taskToken = new TaskTokenCacheModel(taskId);
            _taskTokensCache.Add(taskToken);

            var type = call.Object!.Type;
            Task? task = null;
            task = new Task(() =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var instance = scope.ServiceProvider.GetService(type)
                        ?? scope.ServiceProvider.GetService(type.GetInterfaces()[0]);
                    methodInfo.Invoke(instance, argsValues);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocurrió un error al ejecutar el proceso de backgroundjob. " +
                        "Método {METHOD}", methodInfo.Name);
                }
                finally
                {
                    _taskTokensCache.Remove(taskToken);
                    _logger.LogInformation("Finalizó el proceso de backgroundjob. Método {METHOD}",
                       methodInfo.Name);
                }
            });

            task.Start();
        }

        public bool CancelJob(Guid taskId)
        {
            var taskToken = _taskTokensCache.Find(t => t.TaskId == taskId);
            if (taskToken is null) return false;

            taskToken.Cancel();
            _taskTokensCache.Remove(taskToken);
            return true;
        }

        public CancellationTokenSource? GetJob(Guid taskId)
        {
            return _taskTokensCache.Find(t => t.TaskId == taskId)?.CancellationTokenSource;
        }
    }
}
