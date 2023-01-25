using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freezone.Core.Application.Pipelines.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICachableRequest
    {
        private IDistributedCache _cache;
        private CacheSettings _cacheSettings;

        public CachingBehavior(IDistributedCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>();
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;

            if (request.BypassCache) return await next();

            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                TimeSpan? slidingExpiration =
                request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);
                DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };
                byte[] serializeData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                await _cache.SetAsync(request.CacheKey, serializeData, cacheOptions, cancellationToken);
                return response;
            }

            byte[]? cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

            if (cachedResponse!=null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            }
            else
            {
                response = await GetResponseAndAddToCache();
            }

            return response;
        }
    }
}
