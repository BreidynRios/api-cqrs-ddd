using System.Linq.Expressions;

namespace Application.Interfaces.ServicesClients
{
    public interface IBackgroundJobServiceClient
    {
        void EnqueueJob(Expression<Action> methodCall);
        bool CancelJob(Guid taskId);
        CancellationTokenSource? GetJob(Guid taskId);
    }
}
