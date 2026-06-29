using MeatData.Application.Interfaces.ExternalApis;
using MeatData.Application.Models.ExternalApis.FoodDataCentral;
using MeatData.Infrastructure.ExternalApis.FoodDataCentral;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MeatData.Infrastructure.Cache;

public sealed class CachedFoodDataCentralClient : IFoodDataCentralClient
{
    private readonly IFoodDataCentralClient _inner;
    private readonly IDistributedCache _cache;
    private readonly FoodDataCentralOptions _options;

    public CachedFoodDataCentralClient(
        IFoodDataCentralClient inner,
        IDistributedCache cache,
        IOptions<FoodDataCentralOptions> opts)
    {
        _inner = inner;
        _cache = cache;
        _options = opts.Value;
    }

    public async Task<FoodSearchResult?> SearchFoodsAsync(string query,
        int pageSize = 25, CancellationToken ct = default)
    {
        var key = $"fdc:search:{query.ToLowerInvariant()}:{pageSize}";
        return await GetOrSetAsync(key, _options.CacheHoursForSearch,
            () => _inner.SearchFoodsAsync(query, pageSize, ct), ct);
    }

    public async Task<FoodDetail?> GetFoodByIdAsync(string fdcId, CancellationToken ct = default)
    {
        var key = $"fdc:food:{fdcId}";
        return await GetOrSetAsync(key, _options.CacheHoursForDetail,
            () => _inner.GetFoodByIdAsync(fdcId, ct), ct);
    }

    private async Task<T?> GetOrSetAsync<T>(string key, int hours,
        Func<Task<T?>> factory, CancellationToken ct) where T : class
    {
        var cached = await _cache.GetStringAsync(key, ct);
        if (cached is not null)
            return JsonSerializer.Deserialize<T>(cached);

        var result = await factory();
        if (result is not null)
        {
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(hours)
                }, ct);
        }

        return result;
    }
}