using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

public static class MemoryCacheExtensions
{
    private static readonly ConcurrentDictionary<object, long> hits = new ConcurrentDictionary<object, long>();
    private static readonly ConcurrentDictionary<object, long> misses = new ConcurrentDictionary<object, long>();

    public static TItem GetOrCreateWithStats<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, TItem> factory)
    {
        if (cache.TryGetValue(key, out TItem value))
        {
            hits.AddOrUpdate(key, 1, (k, v) => v + 1);
            return value;
        }
        else
        {
            misses.AddOrUpdate(key, 1, (k, v) => v + 1);
            return cache.Set(key, factory(cache.CreateEntry(key)));
        }
    }

    public static long GetHitCount(this IMemoryCache cache, object key)
    {
        hits.TryGetValue(key, out long count);
        return count;
    }

    public static long GetMissCount(this IMemoryCache cache, object key)
    {
        misses.TryGetValue(key, out long count);
        return count;
    }

    public static long GetTotalHitCount(this IMemoryCache cache)
    {
        return hits.Sum(kvp => kvp.Value);
    }

    public static long GetTotalMissCount(this IMemoryCache cache)
    {
        return misses.Sum(kvp => kvp.Value);
    }
}
