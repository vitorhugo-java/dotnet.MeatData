namespace MeatData.Application.Products.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    string SKU,
    decimal WeightGrams,
    CategoryDto Category,
    NutritionProfileDto? NutritionProfile,
    DateTime CreatedAt,
    DateTime UpdatedAt);
