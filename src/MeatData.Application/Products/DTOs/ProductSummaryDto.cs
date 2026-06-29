namespace MeatData.Application.Products.DTOs;

// Versão leve para listagens — sem o NutritionProfile completo
// Equivalente a uma @Projection ou @JsonView no Spring Boot
public record ProductSummaryDto(
    Guid Id,
    string Name,
    string SKU,
    decimal WeightGrams,
    string CategoryName,
    bool HasNutritionProfile,
    DateTime CreatedAt);
