using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MeatData.Application.Models.ExternalApis.FoodDataCentral
{
    public class FoodNutrient
    {
        [JsonPropertyName("nutrientNumber")]
        public string? Number { get; set; }

        [JsonPropertyName("nutrientName")]

        public string? Name { get; set; }

        [JsonPropertyName("value")]
        public double? Amount { get; set; }

        public string? UnitName { get; set; }
    }
}
