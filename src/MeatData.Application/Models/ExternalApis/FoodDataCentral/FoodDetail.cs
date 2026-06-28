using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Models.ExternalApis.FoodDataCentral
{
    public class FoodDetail
    {
        public int FdcId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? BrandOwner { get; set; }
        public string? BrandName { get; set; }
        public List<FoodNutrient> FoodNutrients { get; set; } = [];
    }
}
