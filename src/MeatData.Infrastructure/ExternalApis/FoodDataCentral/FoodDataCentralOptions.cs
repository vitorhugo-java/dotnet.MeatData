using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeatData.Infrastructure.ExternalApis.FoodDataCentral
{
    public sealed class FoodDataCentralOptions
    {
        public const string SectionName = "FoodDataCentral";

        [Required, Url]
        public string BaseUrl { get; init; } = default!;

        [Required, MinLength(1)]
        public string ApiKey { get; init; } = default!;

        [Range(1, 200)]
        public int DefaultPageSize { get; init; } = 25;

        [Range(1, 24)]
        public int CacheHoursForSearch { get; init; } = 6;

        [Range(1, 168)]
        public int CacheHoursForDetail { get; init; } = 24;
    }
}
