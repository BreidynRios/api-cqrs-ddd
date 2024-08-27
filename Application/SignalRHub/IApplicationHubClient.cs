namespace Application.SignalRHub
{
    public interface IApplicationHubClient
    {
        Task NewJob(string mensaje);
        Task FinishedJob(string mensaje);
    }
}
