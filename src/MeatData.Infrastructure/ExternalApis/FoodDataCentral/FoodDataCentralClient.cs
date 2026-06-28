using MeatData.Application.Interfaces.ExternalApis;
using MeatData.Application.Models.ExternalApis.FoodDataCentral;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace MeatData.Infrastructure.ExternalApis.FoodDataCentral
{
    public sealed class FoodDataCentralClient : IFoodDataCentralClient
    {
        private readonly HttpClient _http;
        private readonly FoodDataCentralOptions _options;

        public FoodDataCentralClient(HttpClient http, IOptions<FoodDataCentralOptions> opts)
        {
            _http = http;
            _options = opts.Value;
        }

        public async Task<FoodSearchResult?> SearchFoodsAsync(string query,
            int pageSize = 25, CancellationToken ct = default)
        {
            var url = $"foods/search?query={Uri.EscapeDataString(query)}&pageSize={pageSize}&api_key={_options.ApiKey}";
            return await _http.GetFromJsonAsync<FoodSearchResult>(url, ct);
        }

        public async Task<FoodDetail?> GetFoodByIdAsync(string fdcId, CancellationToken ct = default)
        {
            var url = $"food/{fdcId}?api_key={_options.ApiKey}";
            return await _http.GetFromJsonAsync<FoodDetail>(url, ct);
        }
    }
}
