using MeatData.Application.Interfaces.ExternalApis;
using MeatData.Application.Models.ExternalApis.FoodDataCentral;
using MeatData.Infrastructure.Cache;
using MeatData.Infrastructure.ExternalApis.FoodDataCentral;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Text.Json;

namespace MeatDataPortal.Tests;

public class CachedFoodDataCentralClientTests
{
    private readonly IFoodDataCentralClient _inner = Substitute.For<IFoodDataCentralClient>();
    private readonly IDistributedCache _cache = Substitute.For<IDistributedCache>();
    private readonly CachedFoodDataCentralClient _sut;

    public CachedFoodDataCentralClientTests()
    {
        var options = Options.Create(new FoodDataCentralOptions
        {
            BaseUrl = "https://api.nal.usda.gov/fdc/v1/",
            ApiKey = "TEST",
            CacheHoursForSearch = 6,
            CacheHoursForDetail = 24
        });

        _sut = new CachedFoodDataCentralClient(_inner, _cache, options);
    }

    [Fact]
    public async Task SearchFoodsAsync_WhenCacheHit_ReturnsCachedResult_WithoutCallingInner()
    {
        // Arrange
        var cached = new FoodSearchResult { TotalHits = 1, Foods = [new() { FdcId = 1, Description = "Cached beef" }] };
        var json = JsonSerializer.Serialize(cached);

        _cache.GetAsync("fdc:search:beef:25", Arg.Any<CancellationToken>())
              .Returns(System.Text.Encoding.UTF8.GetBytes(json));

        // Act
        var result = await _sut.SearchFoodsAsync("beef");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Cached beef", result.Foods[0].Description);
        await _inner.DidNotReceive().SearchFoodsAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchFoodsAsync_WhenCacheMiss_CallsInner_AndPopulatesCache()
    {
        // Arrange
        var apiResult = new FoodSearchResult { TotalHits = 1, Foods = [new() { FdcId = 2, Description = "Fresh beef" }] };

        _cache.GetAsync("fdc:search:beef:25", Arg.Any<CancellationToken>())
              .Returns((byte[]?)null);

        _inner.SearchFoodsAsync("beef", 25, Arg.Any<CancellationToken>())
              .Returns(apiResult);

        // Act
        var result = await _sut.SearchFoodsAsync("beef");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Fresh beef", result.Foods[0].Description);
        await _inner.Received(1).SearchFoodsAsync("beef", 25, Arg.Any<CancellationToken>());
        await _cache.Received(1).SetAsync(
            "fdc:search:beef:25",
            Arg.Any<byte[]>(),
            Arg.Any<DistributedCacheEntryOptions>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchFoodsAsync_WhenInnerReturnsNull_DoesNotPopulateCache()
    {
        // Arrange
        _cache.GetAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
              .Returns((byte[]?)null);

        _inner.SearchFoodsAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
              .Returns((FoodSearchResult?)null);

        // Act
        var result = await _sut.SearchFoodsAsync("beef");

        // Assert
        Assert.Null(result);
        await _cache.DidNotReceive().SetAsync(
            Arg.Any<string>(),
            Arg.Any<byte[]>(),
            Arg.Any<DistributedCacheEntryOptions>(),
            Arg.Any<CancellationToken>());
    }
}