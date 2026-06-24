using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class Product
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string SKU { get; set; }
        decimal WeightGrams { get; set; }
        Guid CategoryId { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }

        private Product() { }

        public static Product Create(string name, string description, string sku, decimal weightGrams, Guid categoryId)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Product name cannot be null or empty.", nameof(name));

            if (weightGrams <= 0) throw new ArgumentException("Weight must be greater than zero.", nameof(weightGrams));

            return new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                SKU = sku,
                WeightGrams = weightGrams,
                CategoryId = categoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
