using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelBooking.Api.Services;

public class RedisCacheWrapper : ISystemCache
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheWrapper(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public Task<bool> TryGet<T>(string key, out T? value)
    {
        var bytes = _distributedCache.Get(key);
        if (bytes == null)
        {
            value = default;
            return Task.FromResult(false);
        }

        var data = Encoding.UTF8.GetString(bytes);
        var result = JsonSerializer.Deserialize<T>(data);
        if (result == null)
        {
            value = default;
            return Task.FromResult(false);
        }

        value = result;
        return Task.FromResult(true);
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
