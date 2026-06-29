using MeatData.Application.Models.ExternalApis.FoodDataCentral;
using MeatData.Infrastructure.ExternalApis.FoodDataCentral;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace MeatDataPortal.Tests;

public class FoodDataCentralClientTests
{
    private const string BaseUrl = "https://api.nal.usda.gov/fdc/v1/";
    private const string FakeApiKey = "TEST_KEY";

    private (FoodDataCentralClient client, MockHttpMessageHandler handler) CreateSut()
    {
        var handler = new MockHttpMessageHandler();
        var httpClient = handler.ToHttpClient();
        httpClient.BaseAddress = new Uri(BaseUrl);

        var options = Options.Create(new FoodDataCentralOptions
        {
            BaseUrl = BaseUrl,
            ApiKey = FakeApiKey
        });

        return (new FoodDataCentralClient(httpClient, options), handler);
    }

    [Fact]
    public async Task SearchFoodsAsync_WhenApiReturnsData_ReturnsResult()
    {
        // Arrange
        var (sut, handler) = CreateSut();

        var fakeResponse = new FoodSearchResult
        {
            TotalHits = 1,
            Foods =
            [
                new FoodSearchItem { FdcId = 174032, Description = "Beef, sirloin" }
            ]
        };

        handler
            .When($"{BaseUrl}foods/search?query=beef&pageSize=25&api_key={FakeApiKey}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fakeResponse));

        // Act
        var result = await sut.SearchFoodsAsync("beef");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalHits);
        Assert.Equal("Beef, sirloin", result.Foods[0].Description);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task SearchFoodsAsync_WhenApiReturns500_ThrowsHttpRequestException()
    {
        // Arrange
        var (sut, handler) = CreateSut();

        handler
            .When($"{BaseUrl}foods/search*")
            .Respond(HttpStatusCode.InternalServerError);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            sut.SearchFoodsAsync("beef"));
    }

    [Fact]
    public async Task GetFoodByIdAsync_WhenFoodExists_ReturnsFoodDetail()
    {
        // Arrange
        var (sut, handler) = CreateSut();

        var fakeDetail = new FoodDetail
        {
            FdcId = 174032,
            Description = "Beef, sirloin",
            FoodNutrients =
            [
                new FoodNutrient { Name = "Protein", Amount = 27.8, UnitName = "g" }
            ]
        };

        handler
            .When($"{BaseUrl}food/174032?api_key={FakeApiKey}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(fakeDetail));

        // Act
        var result = await sut.GetFoodByIdAsync("174032");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(174032, result.FdcId);
        Assert.Single(result.FoodNutrients);
        Assert.Equal(27.8, result.FoodNutrients[0].Amount);
        handler.VerifyNoOutstandingExpectation();
    }
}