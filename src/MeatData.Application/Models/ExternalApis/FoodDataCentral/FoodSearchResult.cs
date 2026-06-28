using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Models.ExternalApis.FoodDataCentral
{
    public class FoodSearchResult
    {
        public List<FoodSearchItem> Foods { get; set; } = [];
        public int TotalHits { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public class FoodSearchItem
    {
        public int FdcId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? BrandOwner { get; set; }
        public string? BrandName { get; set; }
        public List<FoodNutrient> FoodNutrients { get; set; } = [];
    }
}
