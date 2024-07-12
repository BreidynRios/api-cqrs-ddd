namespace Application.Commons.Behaviors.Interfaces
{
    public interface IDataCache
    {
        string CacheKey { get; }
        TimeSpan Expiration { get; }
    }
}
