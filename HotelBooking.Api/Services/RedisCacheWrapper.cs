using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelBooking.Api.Services;

public class RedisCacheWrapper : ISystemCache
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheWrapper(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> Get<T>(string key)
    {
        var bytes = await _distributedCache.GetAsync(key);
        if (bytes == null) return default;

        var data = Encoding.UTF8.GetString(bytes);
        var result = JsonSerializer.Deserialize<T>(data);
        return result;
    }

    public async Task Add<T>(string key, T value)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        await _distributedCache.SetAsync(key, bytes, new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        });
    }
}
