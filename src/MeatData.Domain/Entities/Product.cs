using MeatData.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string SKU { get; private set; }
        public decimal WeightGrams { get; private set; }
        public Guid CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }
        public NutritionProfile? NutritionProfile { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Product() { }

        public static Product Create(string name, string? description, string sku, decimal weightGrams, Guid categoryId)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Product name cannot be null or empty.");

            if (weightGrams <= 0) throw new DomainException("Weight must be greater than zero.");

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
