using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class NutritionProfile
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string FdclId { get; private set; }
        public decimal Calories { get; private set; }
        public decimal ProteinGrams { get; private set; }
        public decimal FatGrams { get; private set; }
        public decimal CarbsGrams { get; private set; }
        public decimal SodiumMg { get; private set; }
        public DateTime FetchedAt { get; private set; }
        public string Source { get; private set; }

        private NutritionProfile() { }
    }
}
