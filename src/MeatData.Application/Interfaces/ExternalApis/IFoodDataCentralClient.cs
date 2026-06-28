using MeatData.Application.Models.ExternalApis.FoodDataCentral;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Interfaces.ExternalApis
{
    public interface IFoodDataCentralClient
    {
        Task<FoodSearchResult?> SearchFoodsAsync(string query, int pageSize = 25, CancellationToken ct = default);
        Task<FoodDetail?> GetFoodByIdAsync(string fdcId, CancellationToken ct = default);
    }
}
