using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services;

public class CacheService(IMemoryCache cache) : ICache
{
    private string SetCacheKey<T>(string key) where T : class => $"{typeof(T).Name}:{key}";
    
    public Task<T> GetAsync<T>(string key, CancellationToken token = default) where T : class 
        => Task.FromResult(cache.TryGetValue(SetCacheKey<T>(key), out T value) ? value : default);

    public Task SetAsync<T>(string key, T value, TimeSpan expiry, CancellationToken token = default) where T : class
        => Task.FromResult(cache.Set(SetCacheKey<T>(key), value, expiry));

    public Task RemoveByPrefixAsync(string prefix, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        cache.Remove(SetCacheKey<string>(key));
        return Task.CompletedTask;
    }
}
