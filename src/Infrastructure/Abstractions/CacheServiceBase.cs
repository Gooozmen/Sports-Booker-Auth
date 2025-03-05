using Microsoft.Extensions.Caching.Distributed;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Abstractions;

public abstract class CacheServiceBase<TManager, TEntity> where TManager : IManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly IUnitOfWork _unitOfWork;
    protected readonly TManager _manager;

    protected CacheServiceBase(IMemoryCache cache, IUnitOfWork unitOfWork)
    {
        _memoryCache = cache;
        _unitOfWork = unitOfWork;
        _manager = _unitOfWork.GetManager<TManager>();
    }

    public async Task<TEntity?> GetFromCacheAsync(Guid key)
    {
        string entityKey = $"{typeof(TEntity).FullName}-{key}";
        
        return _memoryCache.GetOrCreateAsync<TEntity>(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                return _manager.GetByIdAsync(id, cancellationToken);
            });
    }

    public async Task SaveToCacheAsync(string key, TEntity entity, TimeSpan? expiry = null)
    {
        var jsonData = JsonConvert.SerializeObject(entity);
        await _memoryCache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
        });
    }

    public abstract Task<TEntity?> GetEntityAsync(string id);
    public abstract Task<TEntity?> CreateEntityAsync(TEntity entity);
    public abstract Task<bool> DeleteEntityAsync(string id);
}
