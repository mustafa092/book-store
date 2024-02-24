using Microsoft.Extensions.Caching.Memory;

namespace cachingManager.services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, TimeSpan cacheTime)
    {
        _cache.Set(key, value, cacheTime);
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void Set<T>(string cacheKey, List<T> cachedBooks, MemoryCacheEntryOptions cacheEntryOptions)
    {
        _cache.Set(cacheKey, cachedBooks, cacheEntryOptions);
    }
}