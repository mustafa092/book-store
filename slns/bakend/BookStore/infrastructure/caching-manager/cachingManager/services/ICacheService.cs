using Microsoft.Extensions.Caching.Memory;

namespace cachingManager.services;

public interface ICacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan cacheTime);
    bool TryGetValue<T>(string key, out T value);
    void Remove(string key);
    void Set<T>(string cacheKey, List<T> cachedBooks, MemoryCacheEntryOptions cacheEntryOptions);
}