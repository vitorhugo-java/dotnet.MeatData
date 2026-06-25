using MeatData.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Domain.Entities
{
    public class ProductCategory
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        AnimalCategory animalCategory { get; set; }
        DateTime CreatedAt { get; set; }

    }
}
