namespace Infrastructure.ModelsClients.BackgroundJob
{
    public class TaskTokenCacheModel
    {
        public Guid TaskId { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public TaskTokenCacheModel(Guid taskId)
        {
            TaskId = taskId;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public void Cancel() => CancellationTokenSource.Cancel();
    }
}
