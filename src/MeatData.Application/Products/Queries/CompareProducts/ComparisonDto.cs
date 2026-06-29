using MeatData.Application.Products.DTOs;

namespace MeatData.Application.Products.Queries.CompareProducts;

public record ComparisonDto(IReadOnlyList<ProductDto> Products);
