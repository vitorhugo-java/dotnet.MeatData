using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class NutritionProfile
    {
        Guid Id { get; set; }
        Guid ProductId { get; set; }
        string FdclId { get; set; }
        decimal Calories { get; set; }
        decimal ProteinGrams { get; set; }
        decimal FatGrams { get; set; }
        decimal CarbsGrams { get; set; }
        decimal SodiumMg { get; set; }
        DateTime FetchedAt { get; set; }
        string Source { get; set; }

        private NutritionProfile() { }
    }
}
