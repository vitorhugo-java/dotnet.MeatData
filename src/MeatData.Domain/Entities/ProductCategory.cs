using MeatData.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class ProductCategory
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public AnimalCategory animalCategory { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<Product> Product { get; private set; }

        private ProductCategory() { }
    }
}
