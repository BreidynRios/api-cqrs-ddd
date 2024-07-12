using Application.Commons.Behaviors.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Application.Commons.Behaviors
{
    public class DataCachePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IDataCache
    {
        protected readonly IDistributedCache _distributedCache;
        private readonly TimeSpan MAX_DAYS_CACHE = TimeSpan.FromDays(1);

        public DataCachePipelineBehavior(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var result = await _distributedCache.GetStringAsync(request.CacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(result))
            {
                var data = JsonSerializer.Deserialize<TResponse>(result);
                if (data is not null)
                    return data;
            }

            var response = await next();
            if (response is null) return response;

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = request.Expiration,
                AbsoluteExpirationRelativeToNow = MAX_DAYS_CACHE,
            };

            await _distributedCache.SetStringAsync(request.CacheKey, 
                JsonSerializer.Serialize(response), options, cancellationToken);

            return response;
        }
    }
}
