namespace Infrastructure.Interfaces;

public interface ICache
{
    Task<T> GetAsync<T>(string key, CancellationToken token = default) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan expiry,CancellationToken token = default) where T : class;
    Task RemoveByPrefixAsync(string prefix, CancellationToken token = default);
    Task RemoveAsync(string key,CancellationToken token = default);
}