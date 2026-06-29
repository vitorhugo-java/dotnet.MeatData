using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
    string Name,
    string? Description,
    string SKU,
    decimal WeightGrams,
    Guid CategoryId);
}
